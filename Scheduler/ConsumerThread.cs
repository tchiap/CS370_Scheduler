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
    public class ConsumerThread
    {
        // Data members

        // Consumer thread
        protected Thread consumerThread;

        // Boolean to see if thread is running
        protected bool isRunning;
        
        // Array of the number of job types processed
        protected int[] numJobTypesProcessedArr;

        // the job ready queue
        protected ReadyQueue readyQueue;

        // Consumer Busy time
        protected int timeConsumerBusy;

        /**
         * Consumer() constructor
         */
        public ConsumerThread(ReadyQueue jobQueue)
        {

            // Set the argument queue to the data member readyQueue
            readyQueue = jobQueue;

            // Start the Consumer thread
            consumerThread = new Thread( new ThreadStart(StartConsumers) );

            // Initialize the number of job types processed
            numJobTypesProcessedArr = new int[(int)Jobs.JOB_TYPE.NUM_JOB_TYPES];
        }

        /**
         * Stop() method
         * Stops the consumer thread
         */
        public void Stop()
        {
            isRunning = false;
            consumerThread.Abort();
            consumerThread.Join();
        }

        // Return the busy cosumer time
        public int BusyTime
        {
            get
            {
                return timeConsumerBusy;
            }
        }

        // Return the jobs processed array
        public int [] JobsProcessed
        {
            get
            {
                return numJobTypesProcessedArr;
            }
        }

        /**
         * Start the consumer thread
         */ 
        private void StartConsumers()
        {
            
            Jobs job = null;

            // While the thread is running, 
            while (isRunning)
            {

                // Acquire the semaphore
                readyQueue.Semaphore.WaitOne();

                // If the queue job counter is greater the 0, dequeue the job queue
                if (readyQueue.GetJobQueueCount > 0)
                {
                    job = readyQueue.Dequeue();
                }

                // Release the semaphore -- end job data protection
                readyQueue.Semaphore.Release();

                if (job != null)
                {
                    // Set the consumer busy time to the busy time plus the job length
                    timeConsumerBusy += job.JobLength;

                    // Increment the job types proccessed array at index job.Type
                    numJobTypesProcessedArr[(int)job.Type]++;

                    // Have the thread sleep for as long as the user time unit
                    // multiplied by the job length
                    int sleepTime = (int) Program.msATimeUnit * job.JobLength ;
                    Thread.Sleep(sleepTime);
                }

                // Otherwise, sleep nothing
                else
                {
                    Thread.Sleep(0);
                }

                // Set the job to nothing
                job = null;
            }
        }

        /**
         * Start() method
         * Starts the consumer thread
         */
        public void Start()
        {
            isRunning = true;
            consumerThread.Start();
        }

    }
}
