﻿using System;

namespace LegacyApp
{
    //TO ZMIENIAMY
    //ma dzialac tak jak dzialalo przed refaktoryzacja, ma dzialac dokladnie tak samo jak dzialala przed nami
    //testy -> zrefaktoryzowana ta metoda, bo bedziemy musieli pisac testy sprawdzajace czy refaktoryzacja sie powiodla
    public class UserService
    {
        //tylko i wylacznie add user mozemy dodac i usuwac
        //nie mozemy nic usunac, mozemy cos dodac w innych kladach
        public bool AddUser(string firstName, string lastName, string email, DateTime dateOfBirth, int clientId)
        {
            if (!Walidation(firstName, lastName, email, dateOfBirth))
                return false;

            var clientRepository = new ClientRepository();
            var client = clientRepository.GetById(clientId);

            var user = new User
            {
                Client = client,
                DateOfBirth = dateOfBirth,
                EmailAddress = email,
                FirstName = firstName,
                LastName = lastName
            };

            if (!CreditLimit(user, client))
                return false;

            UserDataAccess.AddUser(user);
            return true;
        }

        private bool CreditLimit(User user, Client client)
        {
            if (client.Type == "VeryImportantClient")
            {
                user.HasCreditLimit = false;
            }
            else if (client.Type == "ImportantClient")
            {
                using var userCreditService = new UserCreditService();
                int creditLimit = userCreditService.GetCreditLimit(user.LastName, user.DateOfBirth);
                creditLimit = creditLimit * 2;
                user.CreditLimit = creditLimit;
            }
            else
            {
                user.HasCreditLimit = true;
                using var userCreditService = new UserCreditService();
                int creditLimit = userCreditService.GetCreditLimit(user.LastName, user.DateOfBirth);
                user.CreditLimit = creditLimit;
            }

            if (user.HasCreditLimit && user.CreditLimit < 500)
            {
                return false;
            }
            return true;
        }

        private bool Walidation(string firstName, string lastName, string email, DateTime dateOfBirth)
        {
            if (string.IsNullOrEmpty(firstName) || string.IsNullOrEmpty(lastName))
            {
                return false;
            }

            if (!email.Contains('@') && !email.Contains('.'))
            {
                return false;
            }

            var now = DateTime.Now;
            int age = now.Year - dateOfBirth.Year;
            if (now.Month < dateOfBirth.Month || (now.Month == dateOfBirth.Month && now.Day < dateOfBirth.Day))
                age--;

            if (age < 21)
            {
                return false;
            }

            return true;
        }

    }
}
