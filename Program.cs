using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComputerClubSimulator
{
    internal class Program
    {
        static void Main(string[] args)
        {
            ComputerClub computerClub = new ComputerClub(8);
            computerClub.Work();
        }
    }

    class ComputerClub
    {
        private int _money = 0;
        private List<Computer> _computers = new List<Computer>();    
        private Queue<Client> _clients = new Queue<Client>();

        public ComputerClub(int computersCount)
        {
            Random random = new Random();

            for(int i = 0; i < computersCount; i++)
            {
                _computers.Add(new Computer(random.Next(5, 16)));
            }

            CreateNewClients(25, random);
        }

        public void CreateNewClients(int count, Random random)
        {
            for (int i = 0; i < count; i++)
            {
                _clients.Enqueue(new Client(random.Next(100, 251), random));
            }
        }

        public void Work()
        {
            while (_clients.Count > 0)
            {
                Client newClient = _clients.Dequeue();
                Console.WriteLine($"Computers` club balance {_money} UAH. Wait for clients!");
                Console.WriteLine($"New clients wants to buy {newClient.DesiredMinutes} minutes.");
                ShowAllComputersstate();


                Console.WriteLine("\nYou offer hom computer under the number: ");
                string userInput = Console.ReadLine();
                
                if (int.TryParse(userInput, out int computerNumber))
                {
                    computerNumber -= 1;

                    if (computerNumber >= 0 && computerNumber < _computers.Count)
                    {
                        if (_computers[computerNumber].IsTaken)
                        {
                            Console.WriteLine("You trying to seat a client at computer that is already taken by somebody.");
                        }
                        else
                        {
                            if (newClient.CheckSoulvency(_computers[computerNumber]))
                            {
                                Console.WriteLine("Client payed for a copmuter " + (computerNumber+1) );
                                _money += newClient.Pay();
                                _computers[computerNumber].BecomeTaken(newClient);
                            }
                            else
                            {
                                Console.WriteLine("Client has no money.");
                            }
                        }
                    }
                    else
                    {
                        Console.WriteLine("You even dont know what computer give to client...");
                    }
                }
                else
                {
                    CreateNewClients(1, new Random());
                    Console.WriteLine("Ivalid input. Try again.");
                }

                Console.WriteLine("Press any key to continue work with clients.");
                Console.ReadKey();
                Console.Clear();
                SpendOneMinute();
            }
        }

        private void ShowAllComputersstate()
        {
            Console.WriteLine("\nList of all copmuters: ");
            for (int i = 0; i < _computers.Count; i++)
            {
                Console.Write(i + 1 + " - ");
                _computers[i].ShowState();
            }
        }

        private void SpendOneMinute()
        {
            foreach (var computer in _computers)
            {
                computer.SpendOneMinute();
            }
        }
    }

    class Computer
    {
        private Client _client;
        private int _minutesRemaining;
        public bool IsTaken
        {
            get
            {
                return _minutesRemaining > 0;
            }
        }

        public int PricePerMinute { get;  private set; }

        public Computer( int pricePerMinute)
        {
            
            PricePerMinute = pricePerMinute;
        }

        public void BecomeTaken(Client client)
        {
            _client = client;
            _minutesRemaining = _client.DesiredMinutes;
        }

        public void BecomeEmpty()
        {
            _client = null;
        }

        public void SpendOneMinute()
        {
            _minutesRemaining--;
        }

        public void ShowState()
        {
            if (IsTaken)
                Console.WriteLine($"Computer is taken, minutes remaining: {_minutesRemaining} ");
            else 
                Console.WriteLine($"Computer is free, price per minute: {PricePerMinute}");
        }
    }

    class Client
    {
        private int _money;
        private int _moneyToPay;
        public int DesiredMinutes { get; private set; }

        public Client (int money, Random random)
        {
            _money = money;
            DesiredMinutes = random.Next(10, 30);
        }

        public Client(int v)
        {
        }

        public bool CheckSoulvency(Computer computer)
        {
            _moneyToPay = DesiredMinutes * computer.PricePerMinute;
            if (_money >= _moneyToPay)
            {
                return true;
            }
            else
            {
                _moneyToPay = 0;
                return false;
            }
        }

        public int Pay()
        {
            _money -= _moneyToPay;
            return _moneyToPay;
                
        }
    }
}
