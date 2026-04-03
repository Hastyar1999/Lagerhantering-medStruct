Det här projektet är ett butikssystem skrivet i C#. Programmet körs i konsolen och låter mig hantera ett lager med olika varor. Jag kan lägga till nya artiklar, redigera dem, ta bort dem, sortera, söka och spara allt till fil. Systemet använder en struct (Shop) för varje vara och en lista som fungerar som själva lagret.

Jag har byggt funktioner för att läsa in data från fil, skriva ut alla varor, sortera efter pris eller lagersaldo, söka på olika sätt och ge rekommendationer om lagersaldo börjar bli lågt. Programmet kan också skapa en helt ny fil med standardvärden eller skriva över en befintlig.

Filformatet är enkelt: varje rad sparas med semikolon mellan värdena, t.ex:

1;Sneakers Nike Air;1299;15;2026-03-01

Projektet är gjort för att träna på listor, filhantering, validering av användarinmatning och menyer i C#. Det är också ett bra exempel på hur man bygger ett lite större konsolprogram med flera funktioner som hänger ihop.
