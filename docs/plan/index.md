---
_layout: landing
---

# Terv

<br>

## Megvalósíthatósági terv
- Humán erőforrások: egy termékgazda (13 * 1.5 óra), egy scrum mester (13 * 1.5 óra), négy tervező/fejlesztő/tesztelő (60 óra)
- Hardver erőforrások: négy fejlesztői (közepes hardverigény), GitLab automatikus tesztelő szervere (nem túl erős)
- Szoftver erőforrások: fejlesztőkörnyezet (Visual Studio, Visual Studio Code), verziókövető (GitLab), Git
- Üzemeltetés: üzemeltetést nem kell biztosítani
- Karbantartás: az esetleges hibajavításon felül nem kell biztosítani
- Megvalósítás időtartama ~100 emberóra, költsége fizikailag nem mérhető

<br>

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


<br>

## Használati eset

![use case diagram](https://www.plantuml.com/plantuml/svg/bPQ_ZjGm4CPxFuLrD5p1FS0MwCILIEZ4mGEOP3RhYsD7zWH1q3q4BTsMAQYGU06BzsBnibA6dSo6RVtDR_vySvExJMWY3ftJGuBA950EjgWnw6YR7UhQHgZG1gzIQtrlekbq5toeTZ5qeBV67K9CXI7gzzeaVVKEOfUdjZ5ZRQxKds3Z6mVwOOJOGXhnrrVzHUd3x_dhuBUs6MBULpR_qEcao5E2Q_tipjI0hzm0H_LzthlFvfBgBa-k3nv3AoYVL17Fghk7EDg357nbpQc-Xz5sW_jRtGV0_DFnMQco0xyWozyUTPf9pnHqBd9cLYldRecOQCYCfOmdZPofBwApDq9mUS88kp3cVJBNN3l_IihlWZKtBrxoVMoqzHz32wi0j0u19FfVK7HUD5uC5Smb27wt2eAQGQBu07vFQ3frPdE9P6sbiRmnQuENzfWVba2IxLqFAuTaTaI8bTmDeHX6i1qG3PPNwSOKKe_0_m4gTIbrQL98lT5zX0fgM0lYTvXYn7OBIBhU6msV47ozWi9FYbAKakyHZw82qfwmClzBMucJlvzpeDpawMav_cc-v6_oD08kVC8xQ1x1WS_VZv2ZwArfCU_Z-Qkwv6Rv7-HG8elWBw4y5dvoCdLXFZUhwvBsMgQGGtCBlXuwftu3)

## Nem funkcionális követelmények

- <b>Hatékonság:</b>
    - A pálya méretétől és a robotok számától függ
    - Magas robotszám vagy nagy pályaméret esetén a szimuláció lelassulhat, és a memóriaigény megnőhet
- <b> Megbízhatóság:</b>
    - Szabványos használat esetén nem jelenik meg hibaüzenet, és nincsenek hibák
    - Az emberi tényező miatt lehet hiba, pl. hibás beviteli formátum vagy fájl, ez esetben hibaüzenet jelenik meg.
- <b>Biztonság:</b>
    - A szimulációban nem releváns
    - A való élet beli megvalósítás során fontos lehet, hogy a robotok programjához ne férjenek hozzá illetéktelenül.
- <b>Hordozhatóság:</b>
    - A legtöbb személyi számítógépen futtatható, például Windows 10, 11
    - Azonnal használható, nem szükséges telepíteni
- <b>Felhasználhatóság:</b>
    - Egyszerű, letisztult felhasználó felület, megfelelő instrukciókkal
    - Külön segédlet nem szükséges a használatához
- <b>Környezeti:</b>
    - Nem működik együtt semmilyen külső szoftverrel, szolgáltatással
- <b>Működési:</b>
    - Általában ? futási idő, maximum ? óra
    - gyakori használat
- <b>Fejlesztési:</b>
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
| ![After starting the program](~/images/wireframe/load_in.png) | ![Loading in a log file](~/images/wireframe/load_log_file.png) | ![Config file interface](~/images/wireframe/load_conf.png) |
| ![Config editing](~/images/wireframe/edit_conf.png) | ![After loading in a config file](~/images/wireframe/after_config.png) | ![Simulation runtime](~/images/wireframe/simulation_runtime.png) |
| ![Example for simulation zooming](~/images/wireframe/simulation_zooming.png) | ![Pop-up after the simulation is done](~/images/wireframe/simulation_done.png) | ![After loading in a log file](~/images/wireframe/after_log.png) |
| ![Log replay runtime](~/images/wireframe/log_replay_runtime.png) | ![Log replay paused](~/images/wireframe/log_replay_paused.png) | |





## Felhasználói történetek

<table>
  <tr>
      <td>Eset</td>
      <td></td>
      <td>Leírás</td>
  </tr>
  <tr>
      <td rowspan=3>Új szimuláció</td>
      <td>Given</td>
      <td>Konfiguráció és pálya megadása</td>
  </tr>
  <tr>
      <td>When</td>
      <td>Az alkalmazás egy szimláció létrehozására vár</td>
  </tr>
  <tr>
      <td>Then</td>
      <td>Létrejön egy új szimuláció</td>
  </tr>
  <tr>
      <td rowspan=3>Szimuláció futtatása</td>
      <td>Given</td>
      <td>Létező szimuláció</td>
  </tr>
  <tr>
      <td>When</td>
      <td>A szimuláció futtatásának igénye</td>
  </tr>
  <tr> 
      <td>Then</td>
      <td>A szimuláció fut</td>
  </tr>
  <tr>
      <td rowspan=3>Pálya szerkesztése</td>
      <td>Given</td>
      <td>Létező pálya</td>
  </tr>
  <tr>
      <td>When</td>
      <td>A pálya szerkesztésének igénye</td>
  </tr>
  <tr>
      <td>Then</td>
      <td>A pálya szerkeszthető</td>
  </tr>
  <tr>
      <td rowspan=3>Szimuláció mentése</td>
      <td>Given</td>
      <td>Létező szimuláció</td>
  </tr>
  <tr>
      <td>When</td>
      <td>A szimuláció mentésének igénye</td>
  </tr>
  <tr>
      <td>Then</td>
      <td>A szimuláció mentése megtörténik, a pálya és a konfiguráció mentésével</td>
  </tr>
  <tr>
      <td rowspan=3>Pálya betöltése</td>
      <td>Given</td>
      <td>A pálya paraméterei</td>
  </tr>
  <tr>
      <td>When</td>
      <td>Szimuláció betöltésének igénye</td>
  </tr>
  <tr> 
      <td>Then</td>
      <td>Létrejön egy új pálya a kapott paraméterekkel</td>
  </tr>
  <tr>
      <td rowspan=3>Konfiguráció megjelenítése</td>
      <td>Given</td>
      <td>Egy létező konfiguráció</td>
  </tr>
  <tr>
      <td>When</td>
      <td>Konfiguráció szerkesztésének igénye</td>
  </tr>
  <tr>
      <td>Then</td>
      <td>A konfiguráció megjelenik</td>
  </tr>
  <tr>
      <td rowspan=3>Szimuláció léptetése</td>
      <td>Given</td>
      <td>Létező szimuláció</td>
  </tr>
  <tr>
      <td>When</td>
      <td>A szimuláció fut és léptetni akarjuk</td>
  </tr>
  <tr>
      <td>Then</td>
      <td>A szimuláció léptetése megtörténik</td>
  </tr>
  <tr>
      <td rowspan=3>Célpontok szerkesztése</td>
      <td>Given</td>
      <td>Létező szimuláció</td>
  </tr>
  <tr>
      <td>When</td>
      <td>A szimuláció fut és szerkeszteni akarjuk a célpontot</td>
  </tr>
  <tr>
      <td>Then</td>
      <td>A célpont módosul</td>
  </tr>
  <tr>
      <td rowspan=3>Pálya mentése</td>
      <td>Given</td>
      <td>Létező szimuláció</td>
  </tr>
  <tr>
      <td>When</td>
      <td>A szimulációt el akarjuk menteni</td>
  </tr>
  <tr>
      <td>Then</td>
      <td>A pálya mentése megtörténik</td>
  </tr>
  <tr>
      <td rowspan=3>Pálya létrehozása</td>
      <td>Given</td>
      <td>A pálya paraméterei</td>
  </tr>
  <tr>
      <td>When</td>
      <td>Pálya létrehozásának igénye</td>
  </tr>
  <tr>
      <td>Then</td>
      <td>Létrejön egy új pálya</td>
  </tr>
</table>


## Szerkezeti felépítés

<br>

### Csomag diagram

![package diagram](https://www.plantuml.com/plantuml/svg/LL7DZgCm3BxdAVm2lC5grGLrJqLQXOhBQWuUS9ce8Ob2cAggvjt7nPG9EVdxoMSxEKm9Ovf72vKVWtVaafgknWMCE4BtuffqjmIHkeHkiAHKmEwA0q5hw0OF1NoCInGls17y2K5zxJsrxycknlyRKU94Ru0Jj7KfKcF6sM8otcscnT2qjHWq1OltlQXPpFfjlVQ1bNV9MqjH0giyxwd57r6_HF_kap12d34E9CnPcECdEI6E-Gp_A4vcIkGwWWUiHswK7cE_t5XtvkONCopCslVaXx_6ojESdh5D1KNlRwxu3Lhf72YzWJFxsXf-5NCebXVLEHBnvUNrTGPvOSM_1cncnCOCiKy6uFeeLJJ3Hs9Oxb2r5qyyg4HmgGxa-dcqtm00)

### Osztály
![UML class diagram](https://www.plantuml.com/plantuml/svg/fLTHSzie47xNhs3w92VopBMFdKxRn6ckkMcdcSXtUUZq0KiRcKO23QoS-5pVRpz0ia6O-3npUOWmtyV7xh8iy54XbTWrDPdLLAahHYj9cqGzCjvItIA5wRv0b6pH8TmKWkViRo2_9KdIqkAHLa1U_SNWwHNP98IGdayksfPijbv7pfgkfiW4lsUy54zZz0xQcguTiWt4lueIwl4C--vBge2yUqSN2YKjyFtxunFln_Lgzk_yLizEtfARcMWQoai7p9QKLp2diW8C2N47Jfp10RHJ44NkG7LD7FgZQzeG64Vj5gHY2e4NC8xRJF07LdccJHlQNhFQJAHGCbuvH2jnV053e8t910qnZbu_LOyl3E5TCzUekeCLq3esy48fLl46EEOeAKArFfJPAAJ8YYqv_o9ecROKbtlIHBTw1uUGU8vV2YfBt-_vkerOUNxFIbm6nfr_1bOjCUpzIkqgwC-tnKyey7lsusGphJbE2fTCVTVYv3aZvhF-aKv6scGZMf0KXTnwsUVvzqLKDkwUXvzMm54P6Tiwyn0fXJI8RNY0pbFEGSvP0mOkHRUemSDR2IicEeDTKGJ3Sqve4wnUAbzdHBx9o12nZ3IgSjwF1bZBKZ3_zuPwZ9ok80ZUQI6Ex0bN2eOjOMm3-K0B60Lwb3RtjAvrUL5QimOKlAyl_jON2u7-LhyfhTUwJm8j1Q_NMyyVKsu7_Fb5rgrGpDZf4nrwnIGK0xnlEzZiCOsOZwVO1-V2Si-yn6OAXLp78EjOglDndH6MaP-xjNmoGYwMCy5HYhe6cH7gT2Od2lMLyxz5SeGsdOnBO8fMoUlj2qYABWGUg8ioUrYRI3YXSOaV-ltrjI8dB-_lWjP7LzSP2KPd8KGTSbpCDC5-FyGe_mo-oXrhug0_ERLWp9ymu6GJpAfsWcVZu-jIxwEy1IZ3CfWNIoWxBQK1Kn2aWypF76dJXcD2AsAxrvsFwIGy5WzJePyXuh1BrRZJzamQikNiX5ZCMM6IorQgpvINjUzmtZKpeT2aC8g65JSSeGe89YWXuQHeEay-yHBAq8W8x-K0n_DVxclsY075O-fsxcH8N7Sgp6XTc1d1R2wNaLQituMH0ZFznaqeKmMyjDuAMty_P0Hsdbt8IerM57ZN34BU4bvdv2uZiupSPsGUqbs9z4Pm8N6f9w9A6wb4PtuyWSBau70vdzMYUBnd2ko1x8d1Cm9NkhcD318THv2dZlNDtTzydBLNr_bXMji0ZIw-OLBlAjMuYBg-mpk4ktzPzP6Wo6tcM4-KRPADM86Kh8JjeNdNZ3EbC_b_2zaRnQX8lJvQWk_UnE4m175uT2dq6EuwuUuJSr2etErwN6ii67lbH81BWKEldS4ydiHqUl1qsGF-QoD9WfVGhsdN7exTa-9qNz5Har0bQfRZEnLBFLNMGNgG_icdqPhjvAD3UjIZ9z72FvNc9ZyzMAct1EtttJ1DP4WFX7HRwNOSjvIE4BT3sBZtZvLZ3FjEE4OOxls9PtSSTrVSOTNjtJRedlQLEqc6ylAhZsvO6K9ujDkJv2FmiclgVm40)