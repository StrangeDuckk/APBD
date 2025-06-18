using System.Data;
using Kolokwium_przykladowe.Models.DTOs;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.Data.SqlClient;
using BadRequest = Kolokwium_przykladowe.Exceptions.BadRequest;
using NotFound = Kolokwium_przykladowe.Exceptions.NotFound;

namespace Kolokwium_przykladowe.Services;

public interface IDbService
{
    public Task<IEnumerable<StudentsWithGroupsGetDTO>> GetStudentsWithGroupsAsync(string? firstName);
    public Task<StudentsWithGroupsGetDTO> CreateStudentAsync(StudentCreateDTO student);
    public Task DeteleStudentAsync(int studentId);
    public Task UpdateStudentAsync(int id, StudentUpdateDTO body);
}

public class DbService(IConfiguration configuration): IDbService
{
    private async Task<SqlConnection> GetConnectionAsync()// laczenie z baza
    {
        var connection = new SqlConnection(configuration.GetConnectionString("Default"));
        
        if(connection.State != ConnectionState.Open)
            await connection.OpenAsync();
        
        return connection;
    }
    
    public async Task<IEnumerable<StudentsWithGroupsGetDTO>> GetStudentsWithGroupsAsync(string? searchName)
    {
        var studentsDict = new Dictionary<int, StudentsWithGroupsGetDTO>();// dla grupowania 1:N
        // list dla grupowania 1:1
        
        await using var connection = await GetConnectionAsync();
        
        var sql = """
                  select S.Id, S.FirstName, S.LastName, S.Age, G.Id, G.Name 
                  from Student S
                  left join GroupAssignment GA on S.Id = GA.Student_Id
                  left join "Group" G on GA.Group_Id = G.Id
                  where @SearchName is null or FirstName like '%' + @SearchName + '%';
                  """;//jelis search name jest null to wyswietli wszystkich, jesli nie pokaze tylko tych ktorzy zawieraja to imie
        
        await using var command = new SqlCommand(sql, connection);
        command.Parameters.AddWithValue("@SearchName", searchName is null ? DBNull.Value : searchName);
        
        await using var reader = await command.ExecuteReaderAsync();

        while (await reader.ReadAsync())
        {
            var studentId = reader.GetInt32(0);
            
            if (!studentsDict.TryGetValue(studentId, out var studentDetails)) 
            {
                studentDetails = new StudentsWithGroupsGetDTO
                {
                    Id = studentId,
                    FirstName = reader.GetString(1),
                    LastName = reader.GetString(2),
                    Age = reader.GetInt32(3),
                    Groups = []
                };
                
                studentsDict.Add(studentId, studentDetails);
            }

            if (!await reader.IsDBNullAsync(4))
            {
                studentDetails.Groups.Add(new GroupsGetDTO
                {
                    Id = reader.GetInt32(4),
                    Name = reader.GetString(5),
                });
            }
        }
        
        return studentsDict.Values;
    }
    
    public async Task<StudentsWithGroupsGetDTO> CreateStudentAsync(StudentCreateDTO studentData)
    {
        await using var connection = await GetConnectionAsync();

        var groups = new List<GroupsGetDTO>();
        
        if (studentData.groupAssigements is not null && studentData.groupAssigements.Count != 0)
        {
            foreach (var group in studentData.groupAssigements)
            {
                var groupCheckSql = """
                                    select Id, Name 
                                    from "Group" 
                                    where Id = @Id;
                                    """;

                await using var groupCheckCommand = new SqlCommand(groupCheckSql, connection);
                groupCheckCommand.Parameters.AddWithValue("@Id", group);
                await using var groupCheckReader = await groupCheckCommand.ExecuteReaderAsync();

                if (!await groupCheckReader.ReadAsync())
                {
                    throw new NotFound($"Group with id {group} does not exist");
                }

                groups.Add(new GroupsGetDTO()
                {
                    Id = groupCheckReader.GetInt32(0),
                    Name = groupCheckReader.GetString(1),
                });
            }
        }

        //tansakcja
        await using var transaction = await connection.BeginTransactionAsync();

        try
        {

            var createStudentSql = """
                                   insert into student
                                   output inserted.Id
                                   values (@FirstName, @LastName, @Age);
                                   """;

            await using var createStudentCommand =
                new SqlCommand(createStudentSql, connection, (SqlTransaction)transaction);
            createStudentCommand.Parameters.AddWithValue("@FirstName", studentData.FirstName);
            createStudentCommand.Parameters.AddWithValue("@LastName", studentData.LastName);
            createStudentCommand.Parameters.AddWithValue("@Age", studentData.Age);

            var createdStudentId = Convert.ToInt32(await createStudentCommand.ExecuteScalarAsync());

            foreach (var group in groups)
            {
                var groupAssignmentSql = """
                                         insert into groupAssignment 
                                         values (@StudentId, @GroupId);
                                         """;
                await using var groupAssignmentCommand =
                    new SqlCommand(groupAssignmentSql, connection, (SqlTransaction)transaction);//dodac do transakcji
                groupAssignmentCommand.Parameters.AddWithValue("@StudentId", createdStudentId);
                groupAssignmentCommand.Parameters.AddWithValue("@GroupId", group.Id);

                await groupAssignmentCommand.ExecuteNonQueryAsync();
            }

            await transaction.CommitAsync();

            return new StudentsWithGroupsGetDTO()
            {
                Id = createdStudentId,
                FirstName = studentData.FirstName,
                LastName = studentData.LastName,
                Age = studentData.Age,
                Groups = groups
            };

        }
        catch (Exception)
        {
            await transaction.RollbackAsync();
            throw;
        }
    }

    public async Task DeteleStudentAsync(int studentId)
    {
        await using var connection = await GetConnectionAsync();
        
        await using var transaction = await connection.BeginTransactionAsync();

        try
        {
            //sprawdzenie istnienia studenta
            var checkStudentSql = "select 1 from student where Id = @Id";
            var checkStudentCommand = new SqlCommand(checkStudentSql, connection, (SqlTransaction)transaction);
            checkStudentCommand.Parameters.AddWithValue("@Id", studentId);

            var exits = await checkStudentCommand.ExecuteScalarAsync();
            if (exits is null)
            {
                throw new NotFound($"Student with id {studentId} does not exist");
            }

            // usuniecie przypisan do grup
            var deleteAssigementSql = "delete from GroupAssignment where Student_Id = @StudentId";
            await using var deleteAssigementCommand =
                new SqlCommand(deleteAssigementSql, connection, (SqlTransaction)transaction);
            deleteAssigementCommand.Parameters.AddWithValue("@StudentId", studentId);

            await deleteAssigementCommand.ExecuteNonQueryAsync();

            // ------- usuniecie studenta --------
            var deleteStudentSql = "delete from student where Id = @Id";
            await using var deleteStudentCommand =
                new SqlCommand(deleteStudentSql, connection, (SqlTransaction)transaction);
            deleteStudentCommand.Parameters.AddWithValue("@Id", studentId);

            await deleteStudentCommand.ExecuteNonQueryAsync();

            await transaction.CommitAsync(); //todo pamietac zawsze przy transakcji o zatwierdzeniu
        }
        catch (Exception e)
        {
            await transaction.RollbackAsync();// todo pamietac o rollback przy transakcji
            throw new BadRequest(e.Message);
        }
    }

    public async Task UpdateStudentAsync(int id, StudentUpdateDTO body)
    {
        await using var connection = await GetConnectionAsync();
        await using var transaction = await connection.BeginTransactionAsync();

        try
        {
            // ---------- czy student istnieje -----------
            var checkStudentSql = "select 1 from student where Id = @Id";
            var checkStudentCommand = new SqlCommand(checkStudentSql, connection, (SqlTransaction)transaction);
            checkStudentCommand.Parameters.AddWithValue("@Id", id);
            
            var exits = await checkStudentCommand.ExecuteScalarAsync();// zwraca pierwsza kolumne
            if (exits is null)
            {
                throw new NotFound($"Student with id {id} does not exist");
            }
            
            // --------- aktualizacja danych -----------
            var updates = new List<string>();
            var parameters = new Dictionary<string, object?>();

            if (body.FirstName is not null)
            {
                updates.Add("FirstName = @FirstName");
                parameters["FirstName"]= body.FirstName;
            }
            if (body.LastName is not null)
            {
                updates.Add("LastName = @LastName");
                parameters["LastName"]= body.LastName;
            }

            if (body.Age is not null)
            {
                updates.Add("Age = @Age");
                parameters["Age"]= body.Age;
            }
            
            if(!updates.Any())
                throw new BadRequest("no data provided");
            
            var updateSql = $"Update student SET {string.Join(", ", updates)} where Id = @Id";
            await using var updateCommand = new SqlCommand(updateSql, connection, (SqlTransaction)transaction);
            updateCommand.Parameters.AddWithValue("@Id", id);
            foreach (var parameter in parameters)
                updateCommand.Parameters.AddWithValue("@"+parameter.Key, parameter.Value);
            
            await updateCommand.ExecuteNonQueryAsync();
            await transaction.CommitAsync();
        }
        catch (Exception e)
        {
            await transaction.RollbackAsync();
            throw new BadRequest(e.Message);
        }
    }


    // public async Task<StudentsWithGroupsGetDTO> CreateStudentAsync(StudentCreateDTO student)// moje
    // {
    //     await using var connection = await GetConnectionAsync();
    //     
    //     var groups = new List<GroupsGetDTO>();
    //
    //     if (student.groupAssigements is not null && student.groupAssigements.Any()) // dolaczenie studenta do grupy
    //     {
    //         foreach (var group in student.groupAssigements)
    //         {
    //             var sql = @"select id, name from Group where Id = @Id";
    //             await using var command = new SqlCommand(sql, connection);
    //             command.Parameters.AddWithValue("@Id", group);
    //             await using var reader = await command.ExecuteReaderAsync();
    //
    //             if (!await reader.ReadAsync())
    //                 throw new NotFound($"Group {group} nie istnieje");
    //             
    //             groups.Add(new GroupsGetDTO
    //             {
    //                 Id = reader.GetInt32(0),
    //                 Name = reader.GetString(1),
    //             });
    //             await reader.CloseAsync();
    //         }
    //     }
    //     // ---------------- TRANSAKCJA --------------
    //     await using var transaction = await connection.BeginTransactionAsync(); //albo sie wszystko uda albo nie
    //
    //     try //koniecznie w try catchu
    //     {
    //         var createStudentsql = @"insert into Student values (@firstName, @lastName, @age)
    //                                  select scope_identity();"; 
    //         // ostatni element dodany
    //         await using var command = new SqlCommand(createStudentsql, connection, (SqlTransaction)transaction);
    //         command.Parameters.AddWithValue("@firstName", student.FirstName);
    //         command.Parameters.AddWithValue("@lastName", student.LastName);
    //         command.Parameters.AddWithValue("@age", student.Age);
    //
    //         var createStudentId = Convert.ToInt32(await command.ExecuteScalarAsync()); //przypisanie ostatniego id
    //
    //         foreach (var group in student.groupAssigements)
    //         {
    //             var groupAssigements = @"insert into groupAssigements values (@studentId, @groupId)";
    //             await using var commandCreate =
    //                 new SqlCommand(groupAssigements, connection, (SqlTransaction)transaction);
    //             commandCreate.Parameters.AddWithValue("@studentId", createStudentId);
    //             commandCreate.Parameters.AddWithValue("@groupId", group);
    //
    //             await commandCreate.ExecuteNonQueryAsync();
    //
    //             
    //         }
    //         await transaction.CommitAsync();
    //          
    //         return new StudentsWithGroupsGetDTO
    //         {
    //             Id = createStudentId,
    //             FirstName = student.FirstName,
    //             LastName = student.LastName,
    //             Age = student.Age,
    //             Groups = groups
    //         };
    //     }
    //     catch (Exception ex)
    //     {
    //         await transaction.RollbackAsync();
    //         throw new BadRequest(ex.Message);
    //     }
    // }
}