using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

namespace atm_consola
{
    public class Program
    {
        class Usuario
        {
            public string Nombre { get; set; }
            public int Pin { get; set; }
            public double Saldo { get; set; }
            public List<string> Historial_transacciones { get; set; } = new List<string>();

            public Usuario()
            {
                this.Nombre = Nombre;
                this.Pin = Pin;
                this.Saldo = Saldo;
                this.Historial_transacciones = Historial_transacciones;
            }
        }
        // lista estatica que guarda los datos de los usuarios tipo BD
        static List<Usuario> usuarios = new List<Usuario>();

        public static void Main(string[] args)
        {
            usuarios.Add(new Usuario { Nombre = "Richard", Pin = 321, Saldo = 1000.00 });
            usuarios.Add(new Usuario { Nombre = "Marcos", Pin = 123, Saldo = 1400.00 });

            bool continuar = true;
            while (continuar)
            {
                Usuario usuarioActual = Iniciar();
                if (usuarioActual != null)
                {
                    MostrarMenu(usuarioActual);
                }
                Console.WriteLine("Quieres hacer otra transaccion? (S - N)");
                continuar = Console.ReadLine().ToLower() == "S";
            }
        }

        static Usuario Iniciar()
        {
            int intentos = 0;
            while (intentos < 3)
            {
                Console.WriteLine("Introduzca su numero PIN");
                if (int.TryParse(Console.ReadLine(), out int pin))
                {
                    Usuario usuario = usuarios.Find(u => u.Pin == pin);
                    if (usuario != null)
                    {
                        Console.WriteLine("Bienvenido " + usuario.Nombre);
                        return usuario;
                    }
                    else
                    {
                        intentos++;
                        Console.WriteLine("PIN incorrecto. Intentos restantes: " + (3 - intentos));
                    }
                }
                else
                {
                    Console.WriteLine("PIN no valido. Intentelo de nuevo");
                }
            }

            Console.WriteLine("Ha superado el numero de intentos permitidos");
            return null;
        }

        static void MostrarMenu(Usuario usuario)
        {
            int menu;
            do
            {
                Console.Clear();
                Console.WriteLine("Seleccione una Transaccion:" +
                    "\n 1. Consulta de Saldo"
                    +
                    "\n 2. Deposito de Efectivo"
                    +
                    "\n 3. Retiro de Efectivo"
                    +
                    "\n 4. Historial de Transacciones"
                    +
                    "\n 5. Salir");

                if (int.TryParse(Console.ReadLine(), out menu))
                {
                    switch (menu)
                    {
                        case 1:
                            ConsultarSaldo(usuario);
                            break;
                        case 2:
                            Depositar(usuario);
                            break;
                        case 3:
                            Retirar(usuario);
                            break;
                        case 4:
                            MostrarHistorial(usuario);
                            break;
                        case 5:
                            Console.WriteLine("Gracias por utilizar nuestros servicios");
                            break;
                        default:
                            Console.WriteLine("Por favor, digite un numero del menu");
                            break;
                    }
                }
                else
                {
                    Console.WriteLine("Entrada no valida, por favor intente de nuevo");
                }

                if (menu != 5)
                {
                    Console.WriteLine("Presione cualquier tecla para continuar,,...");
                    Console.ReadKey();
                }
            } while (menu != 5);

        }

        static void ConsultarSaldo(Usuario usuario)
        {
            Console.WriteLine("Su saldo disponible es: C$ " + usuario.Saldo);
            usuario.Historial_transacciones.Add($"Consulta de saldo: C${usuario.Saldo}");
        }

        static void Depositar(Usuario usuario)
        {
            double s_max = 35000.00;
            Console.WriteLine("Digite la cantidad a depositar:");
            if(double.TryParse(Console.ReadLine(), out double deposito))
            {
                if (deposito < s_max)
                {
                    usuario.Saldo += deposito;
                    Console.WriteLine("Deposito exitoso. Su nuevo saldo es: C$ " + usuario.Saldo);
                    usuario.Historial_transacciones.Add($"Deposito de C${deposito}: Nuevo saldo: C${usuario.Saldo}");
                }
                else
                {
                    Console.WriteLine("El deposito excede los limites");
                }
            }
            else
            {
                Console.WriteLine("Cantidad no valida");
            }
        }

        static void Retirar(Usuario usuario)
        {
            Console.WriteLine("Digite la cantidad a retirar:");
            if (double.TryParse(Console.ReadLine(), out double retiro))
            {
                if(usuario.Saldo >= retiro)
                {
                    usuario.Saldo -= retiro;
                    Console.WriteLine("Retiro exitoso. Su nuevo saldo es: C$ " + usuario.Saldo);
                    usuario.Historial_transacciones.Add($"Retiro de C${retiro}: Nuevo saldo: C${usuario.Saldo}");

                }
                else
                {
                    Console.WriteLine("Saldo insuficiente");
                }
                
            }
            else
            {
                Console.WriteLine("Cantidad no valida");
            }
        }

        static void MostrarHistorial(Usuario usuario)
        {
            Console.WriteLine("Historial de Transacciones:");
            if(usuario.Historial_transacciones.Count > 0)
            {
                foreach(var transaccion in usuario.Historial_transacciones)
                {
                    Console.WriteLine(transaccion);
                }
            }
            else
            {
                Console.WriteLine("No hay transacciones en el historial");
            }
        }
    }
}