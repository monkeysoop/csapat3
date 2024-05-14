# Terv

## Megvalósíthatósági terv
- Humán erőforrások: egy termékgazda (13 * 1.5 óra), egy scrum mester (13 * 1.5 óra), négy tervező/fejlesztő/tesztelő (60 óra)
- Hardver erőforrások: négy fejlesztői (közepes hardverigény), GitLab automatikus tesztelő szervere (nem túl erős)
- Szoftver erőforrások: fejlesztőkörnyezet (Visual Studio, Visual Studio Code), verziókövető (GitLab), Git
- Üzemeltetés: üzemeltetést nem kell biztosítani
- Karbantartás: az esetleges hibajavításon felül nem kell biztosítani
- Megvalósítás időtartama ~100 emberóra, ~600.000 huf (gyakornoki fizetésekkel számolva)

## Funkcionális specifikáció
- Általános:
    - Új szimuláció indítása
    - Meglévő szimuláció (log fájl) betöltése, mentése
    - Új konfiguráció létrehozása
    - Konfiguráció betöltése, mentése
    - Konfiguráció szerkesztése
    - Új pálya létrehozása
    - Pálya betöltése, mentése
    - Pálya szerkesztése
    - Kilépés
- Pálya szerkesztése és szimuláció futása alatt:
    - Pálya megjelenítése
- Szimuláció futása közben:
    - Megállítás
    - Léptetés (előre, hátra)
    - Célpontok szerkesztése
    - Szimuláció sebességének állítása
    - Opcionálisan automatikus mentés

## Használati eset

![use case diagram](usecase.svg)

## Felhasználói történetek

<table>
  <tr>
      <td>Eset</td>
      <td></td>
      <td>Leírás</td>
  </tr>
  <tr>
      <td rowspan=3>Új szimuláció</td>
      <td><b>GIVEN</b></td>
      <td>Konfiguráció és pálya megadása</td>
  </tr>
  <tr>
      <td><b>WHEN</b></td>
      <td>Az alkalmazás egy szimláció létrehozására vár</td>
  </tr>
  <tr>
      <td><b>THEN</b></td>
      <td>Létrejön egy új szimuláció</td>
  </tr>
  <tr>
      <td rowspan=3>Szimuláció futtatása</td>
      <td><b>GIVEN</b></td>
      <td>Létező szimuláció</td>
  </tr>
  <tr>
      <td><b>WHEN</b></td>
      <td>A szimuláció futtatásának igénye</td>
  </tr>
  <tr> 
      <td><b>THEN</b></td>
      <td>A szimuláció fut</td>
  </tr>
  <tr>
      <td rowspan=3>Pálya szerkesztése</td>
      <td><b>GIVEN</b></td>
      <td>Létező pálya</td>
  </tr>
  <tr>
      <td><b>WHEN</b></td>
      <td>A pálya szerkesztésének igénye</td>
  </tr>
  <tr>
      <td><b>THEN</b></td>
      <td>A pálya szerkeszthető</td>
  </tr>
  <tr>
      <td rowspan=3>Szimuláció mentése</td>
      <td><b>GIVEN</b></td>
      <td>Létező szimuláció</td>
  </tr>
  <tr>
      <td><b>WHEN</b></td>
      <td>A szimuláció mentésének igénye</td>
  </tr>
  <tr>
      <td><b>THEN</b></td>
      <td>A szimuláció mentése megtörténik, a pálya és a konfiguráció mentésével</td>
  </tr>
  <tr>
      <td rowspan=3>Pálya betöltése</td>
      <td><b>GIVEN</b></td>
      <td>A pálya paraméterei</td>
  </tr>
  <tr>
      <td><b>WHEN</b></td>
      <td>Szimuláció betöltésének igénye</td>
  </tr>
  <tr> 
      <td><b>THEN</b></td>
      <td>Létrejön egy új pálya a kapott paraméterekkel</td>
  </tr>
  <tr>
      <td rowspan=3>Konfiguráció megjelenítése</td>
      <td><b>GIVEN</b></td>
      <td>Egy létező konfiguráció</td>
  </tr>
  <tr>
      <td><b>WHEN</b></td>
      <td>Konfiguráció szerkesztésének igénye</td>
  </tr>
  <tr>
      <td><b>THEN</b></td>
      <td>A konfiguráció megjelenik</td>
  </tr>
  <tr>
      <td rowspan=3>Szimuláció léptetése</td>
      <td><b>GIVEN</b></td>
      <td>Létező szimuláció</td>
  </tr>
  <tr>
      <td><b>WHEN</b></td>
      <td>A szimuláció fut és léptetni akarjuk</td>
  </tr>
  <tr>
      <td><b>THEN</b></td>
      <td>A szimuláció léptetése megtörténik</td>
  </tr>
  <tr>
      <td rowspan=3>Célpontok szerkesztése</td>
      <td><b>GIVEN</b></td>
      <td>Létező szimuláció</td>
  </tr>
  <tr>
      <td><b>WHEN</b></td>
      <td>A szimuláció fut és szerkeszteni akarjuk a célpontot</td>
  </tr>
  <tr>
      <td><b>THEN</b></td>
      <td>A célpont módosul</td>
  </tr>
  <tr>
      <td rowspan=3>Pálya mentése</td>
      <td><b>GIVEN</b></td>
      <td>Létező szimuláció</td>
  </tr>
  <tr>
      <td><b>WHEN</b></td>
      <td>A szimulációt el akarjuk menteni</td>
  </tr>
  <tr>
      <td><b>THEN</b></td>
      <td>A pálya mentése megtörténik</td>
  </tr>
  <tr>
      <td rowspan=3>Pálya létrehozása</td>
      <td><b>GIVEN</b></td>
      <td>A pálya paraméterei</td>
  </tr>
  <tr>
      <td><b>WHEN</b></td>
      <td>Pálya létrehozásának igénye</td>
  </tr>
  <tr>
      <td><b>THEN</b></td>
      <td>Létrejön egy új pálya</td>
  </tr>
</table>

## Nem funkcionális követelmények

- **Hatékonyság:**
    - A pálya méretétől és a robotok számától függ
    - Magas robotszám vagy nagy pályaméret esetén a szimuláció lelassulhat, és a memóriaigény megnőhet
- **Megbízhatóság:**
    - Szabványos használat esetén nem jelenik meg hibaüzenet, és nincsenek hibák
    - Az emberi tényező miatt lehet hiba, pl. hibás beviteli formátum vagy fájl, ez esetben hibaüzenet jelenik meg.
- **Biztonság:**
    - A szimulációban nem releváns
    - A való élet beli megvalósítás során fontos lehet, hogy a robotok programjához ne férjenek hozzá illetéktelenül.
- **Hordozhatóság:**
    - A legtöbb személyi számítógépen futtatható, például Windows 10, 11
    - Azonnal használható, nem szükséges telepíteni
- **Felhasználhatóság:**
    - Egyszerű, letisztult felhasználói felület, megfelelő instrukciókkal
    - Külön segédlet nem szükséges a használatához
- **Környezeti:**
    - Nem működik együtt semmilyen külső szoftverrel, szolgáltatással
- **Működési:**
    - Nagy raktár és sok robot esetén a szimuláció indítása lassú lehet, de később stabilizálódik
    - gyakori használat
- **Fejlesztési:**
    - Git, CI használat
    - Unit Testek
    - Clean Code
    - Dokumentáció
    - C# nyelv, WPF keretrendszer, MVVM architektúra
    - objektumorientált paradigma

<br>

## Wireframe mockup

|   |   |   |
|---|---|---|
| ![After starting the program](wireframe/load_in.png) | ![Loading in a log file](wireframe/load_log_file.png) | ![Config file interface](wireframe/load_conf.png) |
| ![Config editing](wireframe/edit_conf.png) | ![After loading in a config file](wireframe/after_config.png) | ![Simulation runtime](wireframe/simulation_runtime.png) |
| ![Example for simulation zooming](wireframe/simulation_zooming.png) | ![Pop-up after the simulation is done](wireframe/simulation_done.png) | ![After loading in a log file](wireframe/after_log.png) |
| ![Log replay runtime](wireframe/log_replay_runtime.png) | ![Log replay paused](wireframe/log_replay_paused.png) | |

## Szerkezeti felépítés

### Csomag diagram

![package diagram](Component.svg)

### Osztály
![UML class diagram](class.svg)