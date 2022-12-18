FolderOrganisation

Korišten je Visual Studio 2022 MVC5, zajedno sa Microsoft SQL management studio i lokalnim serverom "LocalDb\MSSQLLocalDB"

Nakon preuzimanja projekta potrebno je napraviti sljedeće:

1.U Package Manager NuGet upisati: "Update-Package Microsoft.CodeDom.Providers.DotNetCompilerPlatform -r",
za ažuriranje kompajlera.

*Alternativno se može desni klik na solution, kliknuti na "Clean" zajedno sa "Build" za isti rezultat.

--Sa navedenim postupcima, aplikacija bi trebala ispravno raditi.

Funkcionalnosti:

1.Kretanje po folderima, brisanje, editiranje i kreiranje

2.Pregled structure svih foldera

3.Brisanje cijele baze

4.Restart cijele baze
