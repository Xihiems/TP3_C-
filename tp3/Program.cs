
using TP3;
using System.Linq;
using System;
using System.Collections.Generic;
using System.Threading;

// Yann Raymond
// IT1


namespace tp3
{

    class Program
    {
        private static Mutex mut = new Mutex();
        private const int numThreads = 3;


        static void Main(string[] args)
        {

            // Exercice 1//

            var collection = new MovieCollection().Movies;

            // Count all movie//
            Console.WriteLine("Le total de film présent dans la collection est {0} \n", collection.Count());

            //Count all movie with the letter e //
            var q2 = (from movie in collection where movie.Title.Contains("e") select movie);
            Console.WriteLine("Le nombre de film contenant la lettre e est {0} \n", q2.Count());

            //Count how many time the letter f is in all the titles from this list//
            var nb_f = 0;
            foreach (var movie in collection)
            {
                nb_f += movie.Title.ToLower().Count(x => x == 'f');
            }
            Console.WriteLine("Le nombre de fois ou la lettre f apparait dans les titre de film : {0} \n", nb_f);



            //Display the title of the film with the higher budget//

            var moviesortbybudget = (from movie in collection orderby movie.Budget descending select movie);
            Console.WriteLine("Le film qui à le plus gros budget est {0}\n", moviesortbybudget.First().Title);

            //Display the title of the movie with the lowest box office//

            var moviesortbyboxoffice = (from movie in collection orderby movie.BoxOffice select movie);
            Console.WriteLine("Film qui a le plus petit box office est {0} \n", moviesortbyboxoffice.First().Title);

            //Order the movies by reversed alphabetical order and print the first 11 of the list//

            var moviereversesorttitle = (from movie in collection orderby movie.Title select movie);
            var i = 0;
            Console.WriteLine(" Les 11 premiers film dans l'orde alphabet : \n ");
            foreach (var movie in moviereversesorttitle)
            {
                Console.WriteLine(movie.Title);
                if (i == 10) {
                    break;
                }
                i++;
            }


            //Count all the movies made before 1980/
            var date1980 = new DateTime(1980, 1, 1);
            var moviebefore1980 = (from movie in collection where movie.ReleaseDate < date1980 select movie);
            Console.WriteLine("\nLe nombre de film crée avant 1980 est {0}\n", moviebefore1980.Count());

            //Display the average running time of movies having a vowel as the first letter//

            var time = (from movie in collection where movie.Title.StartsWith("A") || movie.Title.StartsWith("E") || movie.Title.StartsWith("I") || movie.Title.StartsWith("O") || movie.Title.StartsWith("U") || movie.Title.StartsWith("Y") select movie.RunningTime);
            Console.WriteLine("La moyenne des film qui commence par une voyelle est de {0} \n ", time.Average());

            //Print all movies with the letter H or W in the title, but not the letter I or T//

            Console.WriteLine("Voici tous les films qui possède la lettre H ou W mais pas les lettre I et T : \n");
            var filmcontains = (from movie in collection where (movie.Title.ToUpper().Contains("H") || movie.Title.ToUpper().Contains("W")) && (!movie.Title.ToUpper().Contains("T") && !movie.Title.ToUpper().Contains("I")) select movie);
            foreach (var movie in filmcontains)
            {
                Console.WriteLine(movie.Title);
            }

            // Calculate the mean of all Budget / Box Office of every movie ever //
            Console.WriteLine("La moyenne de tout les quotient budget / box office est {0}", collection.Where(x => x.BoxOffice != 0).Sum(x => x.Budget / x.BoxOffice) / collection.Count(x => x.BoxOffice != 0));

            // Exercice 2 //


            Thread th = new Thread(new ParameterizedThreadStart(UseResource));
            th.Name = "Thread 1";

            Thread th2 = new Thread(new ParameterizedThreadStart(UseResource));
            Thread th3 = new Thread(new ParameterizedThreadStart(UseResource));

            th2.Name = "Thread 2";
            th3.Name = "Thread 3";
            int A = 10000;
            int B = 50;
            
            th.Start(new { A, B });
            A = 11000;
            B = 40;
            

            th2.Start(new { A, B });
            A = 9000;
            B = 20;

            th3.Start(new { A, B });


        }

        

        static void UseResource(object time)
        {
            //Pris le code de microsoft page mutex//
            
            dynamic d = time;

            int total = d.A;
            int time_message = d.B;


            Console.WriteLine("{0} is requesting the mutex",
                              Thread.CurrentThread.Name);
            mut.WaitOne();

            Console.WriteLine("{0} has entered the protected area",
                              Thread.CurrentThread.Name);
            int nb_repeat = total / time_message;
            for (var i = 0; i < nb_repeat; i++)
            {

                Console.WriteLine("{0} work", Thread.CurrentThread.Name);
                Thread.Sleep(time_message);
            }
            Console.WriteLine("{0} is leaving the protected area",
                Thread.CurrentThread.Name);

            // Release the Mutex.
            mut.ReleaseMutex();
            Console.WriteLine("{0} has released the mutex",
                Thread.CurrentThread.Name);
        }



    }
        
}
