using System;
using System.Collections.Generic;

public class Program{

  public static List<int> NumerosRepetidos(List<int> lista){
      var cont = new Dictionary<int, int>();
      var repetidos = new List<int>();

      foreach (var num in lista){

          if (cont.ContainsKey(num)){

            if (!repetidos.Contains(num)){
               repetidos.Add(num);
            }
          }
        else cont.Add(num, 1);
      }
      return repetidos;
  }
  public static void Main(){
      var Lista = new List<int> {1, 2, 3, 2, 4, 3, 5, 1, 6, 1};
      var resultado = NumerosRepetidos(Lista);
      Console.WriteLine(string.Join(", ", resultado)); 
  }
}