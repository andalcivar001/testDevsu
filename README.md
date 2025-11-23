# 🚀 TestDevsu

## 🎯 **Objetivo del Proyecto**

Desarrollar una aplicacion web según lo solicitado en el test.El presente proyecto corresponde al Test Técnico de Devsu, desarrollado bajo una arquitectura moderna y utilizando tecnologías actuales para garantizar mantenibilidad, escalabilidad y buenas prácticas tanto en frontend como en backend.

## ✨ Características Principales

- CRUD de entidades solicitadas en el test.
- Frontend moderno construido con Angular 20.
- API REST en .NET 8 con arquitectura por capas.
- Integración con PostgreSQL mediante Entity Framework Core.
- Generación de documentos PDF utilizando QuestPDF.
- Notificaciones elegantes con ngx-toastr.
- Contenedorización del backend mediante Docker.
- Base de datos PostgreSQL.

### Guia Básica

1. Clonar el repositorio:

   ```bash
   git clone https://github.com/andalcivar001/testDevsu.git
   ```

2. Abrir el proyecto frontend y ejecutar:
   ```node_modules
    requiere node v22.18.0 o superior
    ejecutar npm install para instalar dependencias
   ```

## 🛠️ **Guia Docker**

- **Ejecutar los siguientes comandos para Docker:**
  - docker build -t webdevsuapi:latest -f .\WebDevsuAPI\Dockerfile .\
  - docker run -d -p 7103:7103 --name webdevsuapi -e ASPNETCORE_ENVIRONMENT=Development webdevsuapi:latest

## 🖇️ **Contacto**

- **Email:** [alcivar.andres001@hotmail.com]
- **Teléfono:** [0982015000]
- **Portafolio:** [https://andres-alcivar-portafolio.netlify.app/]
