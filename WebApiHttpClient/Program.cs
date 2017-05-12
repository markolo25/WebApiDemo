using System;
using System.Net.Http;
using WebApiHttpClient.Model;

namespace WebApiHttpClient
{
    class Program
    {
        static void Main(string[] args)
        {
            while (true)
            {
                PrintMenu(1);
                switch (Convert.ToInt32(Console.ReadLine()))
                {
                    case 1:
                        Retrieve();
                        break;
                    case 2:
                        Create();
                        break;
                    case 3:
                        Update();
                        break;
                    case 4:
                        Delete();
                        break;
                    default:
                        Console.WriteLine("we don't have that function");
                        break;
                }

            }
            

        }



        static void Create()
        {
            Console.WriteLine("What would you like to name your new student");
            String name = Console.ReadLine();
            var student = new Student() { StudentName = name };
            using(var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:59567/api/");
                var postTask = client.PostAsJsonAsync<Student>("students", student);
                postTask.Wait();

                var result = postTask.Result;
                if (result.IsSuccessStatusCode)
                {

                    var readTask = result.Content.ReadAsAsync<Student>();
                    readTask.Wait();

                    var insertedStudent = readTask.Result;

                    Console.WriteLine("Student {0} inserted ", student.StudentName);
                }
                else
                {
                    Console.WriteLine(result.StatusCode);
                }
            }     


        }
        static void Retrieve()
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:59567/api/");
                //HTTP GET
                var responseTask = client.GetAsync("students");
                responseTask.Wait();

                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {

                    var readTask = result.Content.ReadAsAsync<Student[]>();
                    readTask.Wait();

                    var students = readTask.Result;

                    foreach (var student in students)
                    {
                        Console.WriteLine(student.StudentName +"    " + student.Id);
                    }
                }
            }
        }
        static void Update()
        {
            Student student = null;
            Console.WriteLine("What is the ID of the student you would like to update");
            int id = Convert.ToInt32(Console.ReadLine());
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:59567/api/");
                //HTTP GET
                var responseTask = client.GetAsync("students?id=" + id.ToString());
                responseTask.Wait();

                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsAsync<Student>();
                    readTask.Wait();

                    student = readTask.Result;
                }
            }
            if(student == null)
            {
                Console.WriteLine("ID not found");
            }
            else
            {
                Console.WriteLine("What name would you like this person to have");
                string name = Console.ReadLine();
                student.StudentName = name;
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri("http://localhost:59567/api/students");

                    //HTTP POST
                    var putTask = client.PutAsJsonAsync<Student>("students", student);
                    putTask.Wait();


                    var result = putTask.Result;
                    if (result.IsSuccessStatusCode)
                    {

                        return;
                    }
                }
            }
            return;

        }
        static void Delete()
        {
            Console.WriteLine("What is the id of the Student you want to delete");
            int id = Convert.ToInt32(Console.ReadLine());
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:59567/api/");

                //HTTP DELETE
                var deleteTask = client.DeleteAsync("students/" + id.ToString());
                deleteTask.Wait();

                var result = deleteTask.Result;
                if (result.IsSuccessStatusCode)
                {

                    return;
                }
            }

            return;
        }

        static void PrintMenu(int whichMenu)
        {
            Console.WriteLine("");
            switch (whichMenu)
            {
                case 1:
                    Console.WriteLine("1. Retrieve Student (GET)");
                    Console.WriteLine("2. Create Student (POST)");
                    Console.WriteLine("3. Update Student (PUT)");
                    Console.WriteLine("4. Delete Student (DELETE)");
                    Console.WriteLine("Pick an Option");
                    break;
                case 2:
                    Console.WriteLine("1. By ID");
                    Console.WriteLine("2. By Name");
                    break;
            }


        }
    }
}
