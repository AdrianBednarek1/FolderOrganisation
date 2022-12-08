FolderOrganisation

Korišten je Visual Studio 2022, zajedno sa Microsoft SQL management studio i lokalnim serverom "LocalDb\MSSQLLocalDB"

Nakon preuzimanja projekta potrebno je napraviti sljedeće:

1. U Package Manager NuGet upisati: "Update-Package Microsoft.CodeDom.Providers.DotNetCompilerPlatform -r",
za ažuriranje kompajlera.

2.Obrisati App_Data s desne strane i ponovno kreirati folder s istim nazivom.

3.Obrisati Migration folder, zatim pokrenuti sljedeće naredbe:
    
    enable-migrations
    
    add-migration nameMigration
    
    update-database

Sa navedenim postupcima, aplikacija bi trebala ispravno raditi.

Funkcionalnosti:

1.Kretanje po folderima, brisanje, editiranje i kreiranje

2.Odabir root foldera prema padajućem popisu diskova

3.Ispis strukture foldera od C:\ diska preko CMD-a
