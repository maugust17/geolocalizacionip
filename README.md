# geolocalizacionip
Repositorio con ejercicio de geolocalicación IP. Consta de 2 proyectos, una aplicación para cargar los datos enviando la IP por parametro y un webservice para consultar las estadisticas. Para simplicidad incluí las 2 aplicaciones dentro del mismo contenedor, abajo se describe como acceder al webservice.

El proyecto esta escrito en .Net Core C# 6.0, se persisten los datos en Redis.

-- Bajar los fuentes del repositorio

git clone https://github.com/maugust17/geolocalizacionip.git

-- Acceder a la carpeta con los fuentes

cd geolocalizacionip

-- Para generar los contenedores a partir de los fuentes

docker compose up -d

-- Para ejecutar la app es necesario entrar al contenedor

docker exec -it geolocalizacionip-webapi-1 /bin/sh

-- Luego se pueden usar los scripts para pruebas

./consulta_ip_espania.sh 

./consulta_ip_brasil.sh 

./consulta_ip_argentina.sh 

-- Tambien se puede ejecutar manualmente

dotnet geolocalizacionip.dll <IPv4 o IPv6>

dotnet geolocalizacionip.dll 200.200.200.200

Asegurarse estar dentro de la carpeta /app dentro del contenedor para poder ejecutar la app

# estadisticaapi

Para usar el webservice se expone en el contenedor el puerto 9090.

Desde la misma maquina que se use docker acceder a 

http://127.0.0.1:9090/Estadisticas

A medida que se use la aplicacion par consultar IPs, se van actualizando las estadisticas.

![imagen](https://user-images.githubusercontent.com/26204784/188350659-9828d0c1-6881-48a4-a70a-b456d533a549.png)

# Kubernetes

En los fuentes, dentro de la carpeta k8s se incluye un archivo YAML para desplegar un servicio en Kubernetes. En la image se indica el campo "replicas" que permite escalar la aplicacion para poder recibir más peticiones por minuto.

Para poder desplegarlo, es neesario editar el archivo YAML e indicar el nombre la imagen a utilizar.

![imagen](https://user-images.githubusercontent.com/26204784/188352090-35db0737-3c5b-4823-8e9e-d1a17d31790b.png)

