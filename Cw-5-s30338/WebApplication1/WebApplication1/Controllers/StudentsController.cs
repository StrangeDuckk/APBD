using Microsoft.AspNetCore.Mvc;
using WebApplication1.Models;

namespace WebApplication1.Controllers;

[ApiController]
[Route("[controller]")] // /students
public class StudentsController : ControllerBase
{
    public static List<Student> Students = [
        new Student()
        {
            Id = 1,
            FirstName = "John",
            LastName = "Doe",
            Birthday = new DateTime(1980, 9, 22)
        },
        new Student()
        {
            Id = 2,
            FirstName = "Adam",
            LastName = "Kowalski",
            Birthday = new DateTime(1990, 9, 22)
        }
    ];

    [HttpGet] // /students
    public IActionResult GetAllStudents()
    {
        return Ok(Students);
    }
    
    [HttpGet]
    [Route("{id}")] // /students/1
    public IActionResult GetStudentById(int id)
    {
        var student = Students.FirstOrDefault(e => e.Id == id);

        if (student == null)
        {
            return NotFound($"Student with id: {id} not found");
        }
        
        return Ok(student);
    }

    [HttpPost] // /students
    public IActionResult AddStudent(Student student)
    {
        var nextId = Students.Max(e => e.Id) + 1;
        
        student.Id = nextId;
        Students.Add(student);
        
        return CreatedAtAction(nameof(GetStudentById), new { id = student.Id }, student);
    }

    [HttpPut]
    [Route("{id}")]
    public IActionResult ReplaceStudent(int id, Student student)
    {
        var studentToUpdate = Students.FirstOrDefault(e => e.Id == id);

        if (studentToUpdate == null)
        {
            return NotFound($"Student with id: {id} not found");
        }
        
        studentToUpdate.FirstName = student.FirstName;
        studentToUpdate.LastName = student.LastName;
        studentToUpdate.Birthday = student.Birthday;

        return NoContent();
    }

    [HttpDelete]
    [Route("{id}")]
    public IActionResult DeleteStudent(int id)
    {
        var studentToDelete = Students.FirstOrDefault(e => e.Id == id);

        if (studentToDelete == null)
        {
            return NotFound($"Student with id: {id} not found");
        }
        
        Students.Remove(studentToDelete);
        
        return NoContent();
    }
    
}