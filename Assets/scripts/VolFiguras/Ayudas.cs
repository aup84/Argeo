using System.Collections;
using System.Collections.Generic;


public class Ayudas {
    
    public string ObtenerAyuda(string figura) {
        string mensaje="";
        switch (figura){
            case "Cilindro":
                mensaje = "Cuerpo geométrico formado por una superficie lateral curva y cerrada y dos planos paralelos que forman sus bases circulares";
                break;
            case "Prisma Triangular":
                mensaje = "Un prisma triangular es un poliedro cuya superficie está formada por dos triángulos iguales y paralelos llamados bases y por tres caras laterales que son paralelogramos";
                break;
            case "Prisma Cuadrangular":
                mensaje = "Un prisma cuadrangular es un poliedro cuya superficie está formada por dos cuadriláteros iguales y paralelos llamados bases y por cuatro caras laterales que son paralelogramos.";
                break;
            case "Prisma Pentagonal":
                mensaje = "Un prisma pentagonal es un poliedro cuya superficie está formada por dos pentágonos iguales y paralelos llamados bases y por cinco caras laterales que son paralelogramos.";
                break;
            case "Prisma Hexagonal":
                mensaje = "Un prisma hexagonal es un poliedro cuya superficie está formada por dos hexágonos iguales y paralelos llamados bases y por seis caras laterales que son paralelogramos.";
                break;
            case "Prisma Decagonal":
                mensaje = "Un prisma hexagonal es un poliedro cuya superficie está formada por dos decágonos iguales y paralelos llamados bases y por diez caras laterales que son paralelogramos.";
                break;
        }
        return mensaje;
    }
}
