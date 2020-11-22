## How to start up project

1. Run `npm install` from the `PhotoAlbum.Frontend` folder
2. Open `PhotoAlbum.sln` in Visual Studio and build the `PhotoAlbum.Backend.Dal` project
3. Set `PhotoAlbum.Backend.Dal` as startup project, then run `Update-Database` in Package Manager Console
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
- `npm run swagger`: generates `api/app.generated.ts`
- `npm ci`: reinitialize `node_modules`
- design: https://material.angular.io/
- routing: https://medium.com/@shairez/angular-routing-a-better-pattern-for-large-scale-apps-f2890c952a18
