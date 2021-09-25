# Photo Album

This is a website for managing and sharing photo albums, built with ASP.NET Core backend and Angular frontend.

## Features

- User management with groups support
- Upload and share photos privately or publicly
- Create and manage albums of pictures
- Download whole albums in a compressed format
- Assign and search between various metadata for images
- Write comments on images

## Screenshots

<img src="https://user-images.githubusercontent.com/43880678/134766483-f6f564d9-bd14-42d4-b5bb-ae7134fedd9b.png" width="100%">
<img src="https://user-images.githubusercontent.com/43880678/134766505-c8a1d848-23f2-4e7c-bb52-de281c4caecd.png" width="100%">
<img src="https://user-images.githubusercontent.com/43880678/134766518-d75c2b70-5398-4cd2-816a-283f4a53e385.png" width="100%">

## How to start up project

1. Run `npm install` from the `PhotoAlbum.Frontend` folder
2. Open `PhotoAlbum.sln` in Visual Studio and build the solution
3. Set `PhotoAlbum.Backend.Web` as startup project and `PhotoAlbum.Backend.Dal` as default project in Package Manager Console then run the `Update-Database` command
4. Set `PhotoAlbum.Backend.Web` as startup project, then run it with the `Frontend & Backend` configuration
5. The webapp will open up in a browser window (ignore any errors in the console output), register a new user, or use the pre-generated admin account (configurable in `appsettings.Development.json`) to login

## Useful commands & links

### Backend

- `Backend.Web` \ `Backend Only`: run backend & swagger on `http://localhost:5000/`
- `Backend.Web` \ `Frontend & Backend`: run backend & swagger & angular on `http://localhost:5000/`
- `Backend.Dal` \ `Add-Migration migration-name`: create migration for db changes
- `Backend.Dal` \ `Update-Database`: apply last migration to db

### Frontend

- `npm install`: initialize `node_modules`
- `npm start`: start frontend on `http://localhost:4200/`
- `npm run swagger`: generate `api/app.generated.ts`
- `npm ci`: reinitialize `node_modules`
- design: https://material.angular.io/
- routing: https://medium.com/@shairez/angular-routing-a-better-pattern-for-large-scale-apps-f2890c952a18
