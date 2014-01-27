using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace TaskAutoAssigner
{
    public class Program
    {
        public readonly int MAX_HOURS = 20;
        public List<Task> TaskList;
        public List<Resource> ResourcelList;
        public Dictionary<int, int> Solution;

        public static void Main(string[] args)
        {
            var p = new Program();
            p.ResourcelList = new List<Resource>
            {
                new Resource { ResourceId = 1, ResourceName = "John Smith", ProductivityIndex = 78 }, 
                new Resource {ResourceId = 2, ResourceName = "Bob Dave", ProductivityIndex = 83 }, 
                new Resource { ResourceId = 3, ResourceName = "Andrea Chen",ProductivityIndex = 67 },
                new Resource { ResourceId = 4, ResourceName = "Chris Brown",ProductivityIndex=72}
            };

            p.TaskList = new List<Task>
            {
                new Task{TaskId=1, EstimatedHours = 10}, 
                new Task{TaskId=2,EstimatedHours=7},
                new Task{TaskId=3,EstimatedHours=12},
                new Task{TaskId=4, EstimatedHours=8},
                new Task{TaskId=5, EstimatedHours = 4},
                new Task{TaskId=6, EstimatedHours=6},
                new Task{TaskId = 7, EstimatedHours=4},
                new Task{TaskId=8,EstimatedHours = 11},
                new Task{TaskId=9, EstimatedHours = 7}
            };

            p.Solution = new Dictionary<int, int>();
            //add AsssignedTo as 0 for the moment
            foreach (var task in p.TaskList)
            {
                p.Solution.Add(task.TaskId, task.AssignedTo);
            }

            p.TaskList = p.TaskList.OrderByDescending(item => item.EstimatedHours).ToList();
            p.ResourcelList = p.ResourcelList.OrderByDescending(item => item.ProductivityIndex).ToList();

            foreach (var task in p.TaskList)
            {
                foreach (var resource in p.ResourcelList)
                {
                    int assignedHours = p.GetTotalAssignedHoursForResource(resource.ResourceId, p.Solution, p.TaskList);
                    if (assignedHours + task.EstimatedHours <= p.MAX_HOURS)
                    {
                        Resource res =
                            p.ResourcelList.SingleOrDefault(item => item.ResourceId == p.Solution[task.TaskId]);

                        if (res == null || (res.ProductivityIndex < resource.ProductivityIndex))
                        {
                            p.Solution[task.TaskId] = resource.ResourceId;
                            break;
                        }

                    }
                }
            }

            foreach (var key in p.Solution.Keys)
            {
                Task task = p.TaskList.Single(item => item.TaskId == key);
                Resource resource = p.ResourcelList.Single(item => item.ResourceId == p.Solution[key]);
                Console.WriteLine("Task {0} with {1} hours assigned to {2} with ProdIndex {3}", task.TaskId, task.EstimatedHours, resource.ResourceName, resource.ProductivityIndex);
            
            }
            Console.ReadKey();
        }

        public int GetTotalAssignedHoursForResource(int resourceId, Dictionary<int, int> solution, List<Task> taskList)
        {
            int sum = 0;
            foreach (var key in solution.Keys)
            {
                if (solution[key] == resourceId)
                {
                    Task task = taskList.Single(item => item.TaskId == key);
                    sum += task.EstimatedHours;
                }
            }
            return sum;
        }
    }


    public class Resource
    {
        public int ResourceId { get; set; }
        public string ResourceName { get; set; }
        public decimal ProductivityIndex { get; set; }
        public int AssignedHours { get; set; }
    }


    public class Task
    {
        public int TaskId { get; set; }
        public int EstimatedHours { get; set; }
        public int RealHours { get; set; }
        public int AssignedTo { get; set; }
    }
}

