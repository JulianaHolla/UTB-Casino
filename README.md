# Virtuální kasino UTB

Semestrální projekt do předmětu **Pokročilé webové technologie**.

## 👥 Členové týmu a poměr práce
| Jméno a příjmení | Role v týmu | Poměr práce |
|:---|:---|:---:|
| **Juliana Hollá** | Vedoucí | 1 |
| **Vojtěch Siuda** | Spoluvedoucí | 1 |

---

## 🚀 Spuštění projektu

### Co je potřeba mít nainstalované před spuštěním

| Nástroj | Poznámka |
|:---|:---|
| **.NET SDK 10** | Kontrola: `dotnet --version` |
| **Docker Desktop** | Musí být **spuštěný** |
| Volný port **3307** | Používá ho databáze. Nesmí ho obsazovat jiná aplikace |
| Visual Studio 2026 / JetBrains Rider | Volitelné, aplikaci lze spustit i z terminálu |

MySQL **není potřeba instalovat**, běží v Dockeru. Nástroj `dotnet-ef` se instaluje prvním příkazem níže.

### Příkazy pro spuštění

Zadávejte je po jednom v terminálu `kořenovém adresáři` kde je `docker-compose.yml`:

1. Nainstaluje nástroj pro migraci – stačí to udělat jen jednou; po dokončení otevřete nový terminál 
```
dotnet tool install --global dotnet-ef
```

2. Stáhne a spustí databázi MySQL v Dockeru. Při prvním spuštění chvíli počkejte (cca 20–30 s). Připravenost ověříte příkazem `docker compose logs db` (hláška `ready for connections`, `port: 3306`).
```
docker compose up -d
```
3. Vytvoří databázi z migrací: schéma, číselníky (role, typy transakcí, hry) a účty `admin` a `manager`. Aplikace databázi nikdy sama nevytváří ani nemigruje, proto je tento krok nutný.
```
dotnet ef database update -p UTB.Casino.Infrastructure -s UTB.Casino.Web
```
4. Spustí aplikaci. Nebo ve Visual Studiu místo toho nastavte UTB.Casino.Web jako Start-up projekt a stiskněte F5
```
dotnet run --project UTB.Casino.Web
```

Aplikace pak běží na https://localhost:7100

---

## 📂 Struktura řešení
Vrstvená architektura; závislosti míří dovnitř, **Application a Infrastructure o sobě nevědí** (spojuje je až `Web`):

- `UTB.Casino.Domain`: entity (`Game`, `Bet`, `WalletTransaction`, `TransactionType`), enumy (`Roles`, `GameSlug`, `TransactionTypes`), rozhraní (`IUser`, repozitáře – každé v samostatném souboru, `IUnitOfWork`, `IFileStorage`, `IAccountManager`). **Žádné NuGet balíčky.**
- `UTB.Casino.Application`: služby, herní logika (Blackjack, Ruleta, Automaty), ViewModely, vlastní validační atributy. Žádné Identity.
- `UTB.Casino.Infrastructure`: EF Core + MySQL, `CasinoDbContext`, konfigurace entit, seedování, migrace, repozitáře, `UnitOfWork`, Identity (`User`, `Role`, `AccountIdentityManager` – **jediné místo s `UserManager`/`SignInManager`**).
- `UTB.Casino.Web`: ASP.NET Core MVC – kontrolery, Razor pohledy, administrace (`Areas/Admin`), klientské adaptéry validací.
- `UTB.Casino.Tests`: unit testy herní logiky a kontrolerů (každý test si vytváří vlastní čistou instanci kontroleru i mocků).

## 🛠️ Klíčová implementační rozhodnutí

### 1. Autentizace a autorizace (ASP.NET Core Identity)
Ověřování uživatelů zajišťuje ASP.NET Core Identity zapouzdřené v Infrastructure, role (`Admin`, `Manager`, `Customer`) jsou enum `Roles`:
- Herní a finanční funkce jsou přístupné pouze přihlášeným a ověřeným plnoletým uživatelům (18+) pomocí custom validačního atributu pro kontrolu věku.
- Správa katalogu her, uživatelů a audit všech sázek i transakcí je izolována v administrátorské oblasti `Areas/Admin` a chráněna autorizačním atributem `[Authorize(Roles = nameof(Roles.Admin))]`.

### 2. Herní mechaniky a klientská interaktivita
Aplikace kombinuje bezpečný výpočet na serveru s plynulou odezvou na klientské straně:
- **Ruleta a Automaty:** Veškeré vyhodnocení výsledků a sázek probíhá výhradně na backendu. Frontend pouze vizualizuje vylosovaný stav pomocí JavaScriptových animací a HTML5 Canvasu.
- **Blackjack:** Udržuje stav rozehraného kola hráče napříč jednotlivými tahy (rozdání, Hit, Stand) a dynamicky aktualizuje karty na stole.

### 3. Konzistence peněženky a atomické transakce
Správa zůstatku je striktně kontrolována v aplikační vrstvě:
- Každá sázka a výhra je evidována jako podvojný záznam: změna v tabulce sázek a odpovídající pohyb v peněžence (typy sázka, výhra, vklad).
- Všechny operace probíhají v rámci databázové transakce přes vzor `UnitOfWork`, což garantuje atomicitu a brání vzniku záporného zůstatku při rychlých či souběžných požadavcích.

### 4. Databáze a nasazení
- Databáze je **MySQL 8** běžící v Dockeru (`docker-compose.yml`), schéma vzniká výhradně z EF Core migrací spouštěných mimo aplikaci.
- Uživatelská hesla jsou ukládána jen jako hashe (ASP.NET Core Identity); v repozitáři jsou pouze hashe seedovaných účtů.

---


## 👤 Role a účty

### Role
Role jsou definovány enumem `Roles` a naplněny migrací (tabulka `AspNetRoles`):

| Role | Přístup |
|:---|:---|
| **Admin** | Vše, včetně administrace: správa her, sázky, transakce a **správa uživatelů** (úprava jména, e-mailu, blokace a změna role). |
| **Manager** | Administrace her, přehled všech sázek a transakcí. **Nemá přístup ke správě uživatelů.** |
| **Customer** | Hraje hry a spravuje svou peněženku. Do administrace nemá přístup. Nový zákazník se registruje sám (pouze 18+). |

### Předvytvořené účty
Migrace vytvoří dva účty. V kódu jsou pouze **hashe** hesel, samotná hesla v repozitáři nejsou.

| Přihlašovací jméno | E-mail | Role | Heslo |
|:---|:---|:---|:---|
| `admin` | `admin@casino.local` | Admin | `HJNh5aqwtmHn6Y^3` |
| `manager` | `manager@casino.local` | Manager | `61dziVPkU6605r%4` |


Účty zákazníků vznikají registrací na stránce Register (heslo min. 8 znaků).

---
## ⚠️ Časté chyby a jejich řešení

| Chyba | Příčina a řešení |
|:---|:---|
| `Unable to connect to any of the specified MySQL hosts` | Databáze neběží. Spusťte Docker Desktop a zadejte `docker compose up -d`. Stav zkontrolujete příkazem `docker compose ps`. |
| `failed to connect to the docker API` / `docker daemon is not running` | Docker Desktop není spuštěný. Spusťte ho a počkejte, až naběhne. |
| `Access denied for user 'casino_app'` | Docker volume byl dříve vytvořen s jinými přihlašovacími údaji. Proveďte reset: `docker compose down -v`, poté `docker compose up -d` a znovu `dotnet ef database update -p UTB.Casino.Infrastructure -s UTB.Casino.Web`. |
| `Access denied for user 'casino_app'@'localhost'` | Na portu 3307 pravděpodobně odpovídá jiný (lokálně nainstalovaný) MySQL server. Zjistěte příkazem `netstat -ano \| findstr 3307` (Windows) a ukončete ho. |
| `port is already allocated` / `Bind for 0.0.0.0:3307 failed` | Port 3307 používá jiný program nebo starý kontejner. Ukončete ho, případně `docker compose down` a spusťte znovu. |
| `Could not execute because the specified command or file was not found` (u `dotnet ef`) | Není nainstalovaný `dotnet-ef` nebo nebyl po instalaci otevřen nový terminál. Spusťte `dotnet tool install --global dotnet-ef` a otevřete nový terminál. |
| `Table '...' doesn't exist` | Nebyly aplikovány migrace. Spusťte `dotnet ef database update -p UTB.Casino.Infrastructure -s UTB.Casino.Web`. |
| `no such service: #` | Za příkaz byl zkopírován komentář začínající `#`. Windows `cmd` komentáře nepodporuje, zadávejte příkazy bez nich. |
| Aplikace se připojuje s jinými údaji, než jsou v `appsettings.json` | Přepisuje je uložený User Secret. Zkontrolujte `dotnet user-secrets list --project UTB.Casino.Web` a případně smažte `dotnet user-secrets clear --project UTB.Casino.Web`. |
| Prohlížeč hlásí problém s HTTPS certifikátem | Spusťte `dotnet dev-certs https --trust` a restartujte prohlížeč. |
| Nelze se přihlásit jako `admin` / `manager` | Hesla těchto účtů nejsou v repozitáři, ptejte se autorů. Alternativně se zaregistrujte jako zákazník. |

Podrobnosti o chybě za běhu aplikace najdete v nejnovějším souboru ve složce `UTB.Casino.Web/logs/`.

---

## 📝 Poznámky k odevzdání
- **Stav:** Projekt je plně funkční (obsahuje 3 plnohodnotné hry, peněženku a administraci).
- **Testování:** Unit testy v `UTB.Casino.Tests` ověřují férovost a matematiku herních pravidel a správné reakce systému při nedostatku kreditu.
- **Problémy:** Hlavní výzvou bylo spolehlivé držení stavu rozehraného kola Blackjacku mezi jednotlivými HTTP požadavky a synchronizace vizuální animace s výsledkem vyhodnoceným na serveru.
