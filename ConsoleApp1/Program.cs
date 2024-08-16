using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Classroom__II
{
    class Programa
    {
        // Variable estática para almacenar el último resultado de la operación
        static double[,] resultadoAnterior = null;
        // Variable estática para almacenar el tipo de la última operación realizada
        static string operacionAnterior = "";

        static void Main()
        {
            while (true)
            {
                // Limpiar la consola
                Console.Clear();
                // Mostrar el menú de operaciones
                Console.WriteLine("Menú de Operaciones con Matrices:");
                Console.WriteLine("1. Sumar Matrices");
                Console.WriteLine("2. Restar Matrices");
                Console.WriteLine("3. Multiplicar Matrices");
                Console.WriteLine("4. Mostrar Última Matriz Resultante");
                Console.WriteLine("5. Salir");
                Console.Write("Seleccione una opción: ");

                // Leer la opción seleccionada por el usuario
                int opcion;
                if (!int.TryParse(Console.ReadLine(), out opcion) || opcion < 1 || opcion > 5)
                {
                    Console.WriteLine("Opción inválida. Por favor, seleccione una opción válida.");
                    continue;
                }

                // Salir del programa si la opción es 5
                if (opcion == 5)
                {
                    Console.WriteLine("¡Hasta luego!");
                    break;
                }

                // Mostrar el último resultado si la opción es 4
                if (opcion == 4)
                {
                    MostrarResultadoAnterior();
                    Console.Write("¿Desea realizar otro cálculo? (s/n): ");
                    if (Console.ReadLine().Trim().ToLower() != "s")
                    {
                        Console.WriteLine("¡Hasta luego!");
                        break;
                    }
                    continue;
                }

                // Leer las dimensiones de la primera matriz
                Console.Write("Ingrese el número de filas de la primera matriz: ");
                int filas1 = LeerEnteroPositivo();
                Console.Write("Ingrese el número de columnas de la primera matriz: ");
                int columnas1 = LeerEnteroPositivo();

                // Leer la primera matriz
                double[,] matriz1 = LeerMatriz(filas1, columnas1, "primera");

                double[,] matriz2;
                int filas2, columnas2;

                if (opcion != 3) // Si la operación no es multiplicación (opción 3)
                {
                    // Leer las dimensiones de la segunda matriz
                    Console.Write("Ingrese el número de filas de la segunda matriz: ");
                    filas2 = LeerEnteroPositivo();
                    Console.Write("Ingrese el número de columnas de la segunda matriz: ");
                    columnas2 = LeerEnteroPositivo();

                    // Leer la segunda matriz
                    matriz2 = LeerMatriz(filas2, columnas2, "segunda");

                    if (opcion == 1) // Suma de matrices
                    {
                        if (filas1 != filas2 || columnas1 != columnas2)
                        {
                            Console.WriteLine("Las dimensiones de las matrices no coinciden para la suma.");
                            continue;
                        }

                        resultadoAnterior = SumarMatrices(matriz1, matriz2);
                        operacionAnterior = "Suma";
                    }
                    else // Resta de matrices
                    {
                        if (filas1 != filas2 || columnas1 != columnas2)
                        {
                            Console.WriteLine("Las dimensiones de las matrices no coinciden para la resta.");
                            continue;
                        }

                        resultadoAnterior = RestarMatrices(matriz1, matriz2);
                        operacionAnterior = "Resta";
                    }
                }
                else // Multiplicación de matrices
                {
                    // Leer las dimensiones de la segunda matriz
                    Console.Write("Ingrese el número de filas de la segunda matriz: ");
                    filas2 = LeerEnteroPositivo();
                    Console.Write("Ingrese el número de columnas de la segunda matriz: ");
                    columnas2 = LeerEnteroPositivo();

                    if (columnas1 != filas2)
                    {
                        Console.WriteLine("El número de columnas de la primera matriz debe ser igual al número de filas de la segunda matriz para la multiplicación.");
                        continue;
                    }

                    // Leer la segunda matriz
                    matriz2 = LeerMatriz(filas2, columnas2, "segunda");
                    resultadoAnterior = MultiplicarMatrices(matriz1, matriz2);
                    operacionAnterior = "Multiplicación";
                }

                // Mostrar la matriz resultante y guardarla en un archivo
                MostrarMatriz(resultadoAnterior);
                GuardarMatrizEnArchivo(resultadoAnterior);
                Console.WriteLine("La matriz resultante del cálculo elegido fue almacenada en el archivo resultante.txt");

                // Preguntar al usuario si desea realizar otro cálculo
                Console.Write("¿Desea realizar otro cálculo? (s/n): ");
                if (Console.ReadLine().Trim().ToLower() != "s")
                {
                    Console.WriteLine("¡Hasta luego!");
                    break;
                }
            }
        }

        // Método para leer un entero positivo del usuario
        static int LeerEnteroPositivo()
        {
            int valor;
            while (!int.TryParse(Console.ReadLine(), out valor) || valor <= 0)
            {
                Console.Write("Valor inválido. Ingrese un número entero positivo: ");
            }
            return valor;
        }

        // Método para leer una matriz del usuario
        static double[,] LeerMatriz(int filas, int columnas, string nombreMatriz)
        {
            double[,] matriz = new double[filas, columnas];
            Console.WriteLine($"Ingrese los datos de la {nombreMatriz} matriz ({filas}x{columnas}):");
            for (int i = 0; i < filas; i++)
            {
                for (int j = 0; j < columnas; j++)
                {
                    Console.Write($"Elemento [{i}, {j}]: ");
                    while (!double.TryParse(Console.ReadLine(), out matriz[i, j]))
                    {
                        Console.Write("Valor inválido. Ingrese un número: ");
                    }
                }
            }
            return matriz;
        }

        // Método para mostrar una matriz en la consola
        static void MostrarMatriz(double[,] matriz)
        {
            if (matriz == null) return;

            Console.WriteLine("Resultado:");
            int filas = matriz.GetLength(0);
            int columnas = matriz.GetLength(1);
            for (int i = 0; i < filas; i++)
            {
                for (int j = 0; j < columnas; j++)
                {
                    Console.Write($"{matriz[i, j],10:F2} ");
                }
                Console.WriteLine();
            }
        }

        // Método para guardar una matriz en un archivo de texto
        static void GuardarMatrizEnArchivo(double[,] matriz)
        {
            using (StreamWriter sw = new StreamWriter("resultante.txt"))
            {
                int filas = matriz.GetLength(0);
                int columnas = matriz.GetLength(1);
                for (int i = 0; i < filas; i++)
                {
                    for (int j = 0; j < columnas; j++)
                    {
                        sw.Write($"{matriz[i, j],10:F2} ");
                    }
                    sw.WriteLine();
                }
            }
        }

        // Método para mostrar el último resultado y la última operación realizada
        static void MostrarResultadoAnterior()
        {
            if (resultadoAnterior == null)
            {
                Console.WriteLine("No hay resultados previos.");
                return;
            }

            Console.WriteLine($"Última operación realizada: {operacionAnterior}");
            MostrarMatriz(resultadoAnterior);
        }

        // Método para sumar dos matrices
        static double[,] SumarMatrices(double[,] a, double[,] b)
        {
            int filas = a.GetLength(0);
            int columnas = a.GetLength(1);
            double[,] resultado = new double[filas, columnas];
            for (int i = 0; i < filas; i++)
            {
                for (int j = 0; j < columnas; j++)
                {
                    resultado[i, j] = a[i, j] + b[i, j];
                }
            }
            return resultado;
        }

        // Método para restar dos matrices
        static double[,] RestarMatrices(double[,] a, double[,] b)
        {
            int filas = a.GetLength(0);
            int columnas = a.GetLength(1);
            double[,] resultado = new double[filas, columnas];
            for (int i = 0; i < filas; i++)
            {
                for (int j = 0; j < columnas; j++)
                {
                    resultado[i, j] = a[i, j] - b[i, j];
                }
            }
            return resultado;
        }

        // Método para multiplicar dos matrices
        static double[,] MultiplicarMatrices(double[,] a, double[,] b)
        {
            int filasA = a.GetLength(0);
            int columnasA = a.GetLength(1);
            int filasB = b.GetLength(0);
            int columnasB = b.GetLength(1);
            double[,] resultado = new double[filasA, columnasB];
            for (int i = 0; i < filasA; i++)
            {
                for (int j = 0; j < columnasB; j++)
                {
                    resultado[i, j] = 0;
                    for (int k = 0; k < columnasA; k++)
                    {
                        resultado[i, j] += a[i, k] * b[k, j];
                    }
                }
            }
            return resultado;
        }
    }
}

