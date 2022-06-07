# memoTest



[![MemoTest](https://img.youtube.com/vi/vNsIbRw95Bk/0.jpg)](https://youtu.be/vNsIbRw95Bk?t=6)



El juego fue completamente desarrolado por mi, menos el diseño del UI.


Dentro del github pueden encontrar los diferentes <a href="https://github.com/luisoneto/memoTest/tree/main/memoTest/Assets/Script">scripts</a>, 
donde el <a href="https://github.com/luisoneto/memoTest/blob/main/memoTest/Assets/Script/gameController.cs">gameController</a> es el principal, se encarga
de comunicarse con el script <a href="https://github.com/luisoneto/memoTest/blob/main/memoTest/Assets/Script/CardRotate.cs"> CardRotate</a>,cuando se pueden
dar vuelta o no las cartas, instanciar las cartas,etc... Prácticamente el estado del juego en todo momento.

• <a href="https://github.com/luisoneto/memoTest/blob/main/memoTest/Assets/Script/CardRotate.cs">CardRotate<a/> se encarga del estado en el cual la carta se encuentra, (1 = oculta, 2= rotando, 3 = rotada esperando volver, 4 = volviendo , 5 = acertada)

• <a href="https://github.com/luisoneto/memoTest/blob/main/memoTest/Assets/Script/CanvasController.cs">CanvasController</a> se encarga de actualizar los puntos y las animaciones
de correcto/incorrecto .

• <a href="https://github.com/luisoneto/memoTest/blob/main/memoTest/Assets/Script/Intensity.cs">Intensity<a/> / <a href="https://github.com/luisoneto/memoTest/blob/main/memoTest/Assets/Script/CelebrationStarter.cs">CelebrationStarter</a>. Según el puntaje que obtengas al finalizar el juego
  existen diferentes tipos de intensidad del festejo. Se incrementan segun el puntaje que obtengas. <a href="https://github.com/luisoneto/memoTest/blob/main/memoTest/Assets/Script/Intensity.cs">Intensity<a/> almacena un int en una variable segun el puntaje obtenido y <a href="https://github.com/luisoneto/memoTest/blob/main/memoTest/Assets/Script/CelebrationStarter.cs">CelebrationStarter</a>
  toma usa esa variable para usarla como parámetro de la intensidad de los festejos. 
  
• <a href="https://github.com/luisoneto/memoTest/blob/main/memoTest/Assets/Script/ConfettiScript.cs">ConfettiScript</a> / <a href="https://github.com/luisoneto/memoTest/blob/main/memoTest/Assets/Script/ballonMovement.cs">BallonMovement</a> / <a href="https://github.com/luisoneto/memoTest/blob/main/memoTest/Assets/Script/FireworksController.cs">FireWorksController</a> son los scripts que controlan 
  en que posición se instancian los GameObjects y de que manera se comportan.
 
  
  
  
 
