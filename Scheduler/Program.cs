/**
 * Tom Chiapete
 * Nov 8, 2007
 * FCFS Scheduling Assignment
 * 
 */

using System.Text;
using System.Threading;
using System;
using System.Collections.Generic;
using System.Data.Common;


namespace Scheduler
{
    class Program
    {
        
        // Data members

        // Time unit in milliseconds
        public static int msATimeUnit;
        
        // The number of user inputted consumers
        protected static int numConsumers;

        // Total run time as given my user
        protected static int totalRunTime;

        // Consumer array
        protected static ConsumerThread[] consumerArr;
        
        // Producer Array
        protected static ProducerThread[] producersArr;
        
        // The ready job queue
        protected static ReadyQueue jobQueue;

        // Default number of producers - 2
        protected const int DEFAULT_PRODUCERS = 2;


        /**
         * SetupProducerConsumer() method.
         * Prompts the user for emulation variables.
         * Initialize the producer and consumer arrays.
         */
        public static void SetupProducerConsumer()
        {

            // Set variables to zero
            totalRunTime = 0;

            // Output the default number of producers
            Console.Out.WriteLine("Producers: "+DEFAULT_PRODUCERS);

            numConsumers = 0;
            
            /////////  INPUT CHECKS

            // Loop until valid data
            while (numConsumers <= 0 )
            {
                // Prompt consumer number input
                Console.Out.Write("Consumers: ");
                int.TryParse(Console.ReadLine(), out numConsumers);

                // If less than 1, error.  Continue while loop.
                if (numConsumers <= 0)
                {
                    Console.Out.WriteLine("Please enter a number of consumers 1 or greater.");
                }
            }

            // Loop until valid data
            while (msATimeUnit <= 0)
            {
                // Prompt millisecond/time unit input
                Console.Out.Write("Enter number of milliseconds per time unit: ");
                int.TryParse(Console.ReadLine(), out msATimeUnit);

                // If less than 1, error.  Continue while loop.
                if (msATimeUnit <= 0)
                {
                    Console.Out.WriteLine("Please enter a time unit greater than zero.");
                }
            }

            // Loop until valid data
            while (totalRunTime <= 0)
            {

                // Prompt run time
                Console.Out.Write("Enter a run time: ");
                int.TryParse(Console.ReadLine(), out totalRunTime);

                // If less than or equal to zero, it is not a valid run time.  
                //Continue loop.
                if (totalRunTime <= 0)
                {
                    Console.Out.WriteLine("Invalid number of time units.");
                }
            }

            // Initialize the size of producer thread array - to constant zero
            producersArr = new ProducerThread[DEFAULT_PRODUCERS];

            // Initialize the size of consumer thread array - to user input
            consumerArr = new ConsumerThread[numConsumers];
            
            Jobs.JOB_TYPE[] jobTypes = new Jobs.JOB_TYPE[] 
                        { Jobs.JOB_TYPE.TYPE_1, Jobs.JOB_TYPE.TYPE_2 };

            // Initialize our job ready queue
            jobQueue = new ReadyQueue();

            // Set up the consumers array for the consumer threads
            for (int x = 0; x < consumerArr.Length; x++)
            {
                consumerArr[x] = new ConsumerThread(jobQueue);
            }

            // Set up the producer array for the producer threads
            for (int y = 0; y < producersArr.Length; y++)
            {
                producersArr[y] = new ProducerThread(jobQueue, jobTypes[y] );
            }

            Console.Out.WriteLine("----------------------------------");
            Console.Out.WriteLine("Started emulation.");
            Console.Out.WriteLine();
        }

        /**
         * StartEmulator() method
         * Start producer and consumer threads.
         */
        public static void StartEmulator()
        {
            // Start consumers.
            for (int x = 0; x < consumerArr.Length; x++)
            {
                consumerArr[x].Start();
            }

            // Start producers.
            for (int y = 0; y < producersArr.Length; y++)
            {
                producersArr[y].Start();
            }
        }

        /**
         * main() method
         * Calls a method to initialize the producer and consumer arrays.
         * One it is set up, the emulation method can be called.
         * Lastly, output useful statistics.
         */
        public static void Main( string [] args)
        {

            // Set up producer and consumer arrays
            SetupProducerConsumer();

            // Start emulation
            StartEmulator();

            // Calculate total sleep time
            int totalSleepTime = totalRunTime * msATimeUnit;
            Thread.Sleep(totalSleepTime);

            // End emulation
            EndSim();
            

            // Start statistics
            Console.Out.WriteLine();
            int jobCount = jobQueue.GetJobQueueCount;
            Console.Out.WriteLine("Jobs still in queue: " + jobCount);
            Console.Out.WriteLine();
            
            // Get number of jobs in queue
            int jobsProduced = jobQueue.GetNumJobsProduced;
            Console.Out.WriteLine("Established jobs in queue: " + jobsProduced);

            for (int x = 0; x < producersArr.Length; x++)
            {
                Console.Out.WriteLine("Type " + (x + 1) + " jobs produced: "
                    + producersArr[x].GetJobsProduced);
            }

            Console.Out.WriteLine();
            Console.Out.WriteLine("Number of jobs served: " + jobQueue.GetNumJobsServed);

            // Calculate throughput by taking the total number of jobs served by the jobs produced.
            // Then output throughput percentage.
            double throughput = (double)jobQueue.GetNumJobsServed / jobQueue.GetNumJobsProduced * 100;
            Console.Out.WriteLine("Throughput Percentage: " +  throughput + "%");
            Console.Out.WriteLine();

            // For each consumer processing jobs...
            for (int i = 0; i < numConsumers; i++)
            {
                Console.Out.WriteLine("Consumer " + (i + 1) + " time processing jobs: "
                    + consumerArr[i].BusyTime);

                // ...Show the processing time for each type of process
                for (int j = 0; j < (int)Jobs.JOB_TYPE.NUM_JOB_TYPES; j++)
                {
                    Console.Out.WriteLine("Type " + (j + 1) + " jobs processed: " 
                        + consumerArr[i].JobsProcessed[j]);
                }

                Console.Out.WriteLine((((double)consumerArr[i].BusyTime * 100) 
                                            / totalRunTime) + "% CPU utilization.");
            }
            Console.Out.WriteLine("--------------------");

            // Show min, max, and average wait times for each job type
            for (int z = 0; z < (int)Jobs.JOB_TYPE.NUM_JOB_TYPES; z++)
            {
                
                // Show max wait time for each job type
                Console.Out.WriteLine("Type " + (z + 1) + " MAX wait time:  " +
                    (jobQueue.GetMaxWaitTimes[z] / msATimeUnit));

                // Show average wait time for each job type
                Console.Out.WriteLine("Type " + (z + 1) + " Job Average wait time:  " +
                    ((
                    (double)jobQueue.GetTotalWaitTimes[z] /
                    msATimeUnit) /
                    jobQueue.GetNumTotalJobsProcessed[z]));
                

                // Show miniumum wait time for each job type
                Console.Out.WriteLine("Type " + (z + 1) +" MIN wait time:  " +
                    (jobQueue.GetMinWaitTimes[z] / msATimeUnit));

                Console.Out.WriteLine();
            }

            Console.Out.WriteLine("Done.");
            Console.Out.WriteLine();
            Console.ReadKey();
        }

        
        /**
         * End all the threads for all consumers and producers, 
         * thus stopping the emulation.
         */
        public static void EndSim()
        {

            // Stop all consumer threads
            for (int x = 0; x < consumerArr.Length; x++)
            {
                consumerArr[x].Stop();
            }

            // Stop all the producers.
            for (int y = 0; y < producersArr.Length; y++)
            {
                producersArr[y].Stop();
            }
        }
    }
}
