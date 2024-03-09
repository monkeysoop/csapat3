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
- Megvalósítás időtartama ~100 emberóra, ~600.000 huf (gyakornoki fizetésekkel számolva)

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
| ![After starting the program](~/images/wireframe/load_in.png) | ![Loading in a log file](~/images/wireframe/load_log_file.png) | ![Config file interface](~/images/wireframe/load_conf.png) |
| ![Config editing](~/images/wireframe/edit_conf.png) | ![After loading in a config file](~/images/wireframe/after_config.png) | ![Simulation runtime](~/images/wireframe/simulation_runtime.png) |
| ![Example for simulation zooming](~/images/wireframe/simulation_zooming.png) | ![Pop-up after the simulation is done](~/images/wireframe/simulation_done.png) | ![After loading in a log file](~/images/wireframe/after_log.png) |
| ![Log replay runtime](~/images/wireframe/log_replay_runtime.png) | ![Log replay paused](~/images/wireframe/log_replay_paused.png) | |

## Szerkezeti felépítés

<br>

### Csomag diagram

![package diagram](https://www.plantuml.com/plantuml/svg/LL7DZgCm3BxdAVm2lC5grGLrJqLQXOhBQWuUS9ce8Ob2cAggvjt7nPG9EVdxoMSxEKm9Ovf72vKVWtVaafgknWMCE4BtuffqjmIHkeHkiAHKmEwA0q5hw0OF1NoCInGls17y2K5zxJsrxycknlyRKU94Ru0Jj7KfKcF6sM8otcscnT2qjHWq1OltlQXPpFfjlVQ1bNV9MqjH0giyxwd57r6_HF_kap12d34E9CnPcECdEI6E-Gp_A4vcIkGwWWUiHswK7cE_t5XtvkONCopCslVaXx_6ojESdh5D1KNlRwxu3Lhf72YzWJFxsXf-5NCebXVLEHBnvUNrTGPvOSM_1cncnCOCiKy6uFeeLJJ3Hs9Oxb2r5qyyg4HmgGxa-dcqtm00)

### Osztály
![UML class diagram](https://www.plantuml.com/plantuml/svg/fLTFRzis5B_hKn3jHOvbOEsn2D8wJhDcQ54ZzfP3qGCjlSgi9L8WAQUUwttsYRIiHr7qvc4vHFBx_VZ-yv5wfpILkYuBCYkeKdUCvfAMaTemNj7cZQH6FDLQibMjuI4LVC7-1F9B54KLJJSq1_BY3mRFByW-8eGS-MHQLMH_S0GBLjO5rKpm9yOpyTn77w4gw0wHWwVyayYWw5lenBTK0Rc-fYkb9KtrpSq_6-xM6MtjttZUM2TloCDCb2Nb6GApDUKvBAdCGVk4S03_rSASrWg2o2EekWn3lzLbvGFzhCr1AgOqy1JwUPi9_etbZckJabRth326b9QCvuWqchW-WscWfUc41c9SEtAgDcUMyBfSDgXwX2tG8hHmgXJBUGbSBxIa6lBTKCrUQQfPUY3ZZu9cSKNrkYEDo9ism3ObJk2tWihCZNkvgm9UZfzOfjUUS-CFmFArzgMVgAr2y_Yy-Wwf_f9ylTXDwmwgqckclZJAoO-4cCVkQppgwIPxKO6aMiY3qtrEV1z5RlBkHlX-2rmheyQ-NJc8b48QnBuuGDEdd8DSiX8Cd8bwLO23LnAsJDG6kwCQ30-3Q5om-cBvAY7oTKBQZ2MaL3b6trzWF8l1_EySTHWBkWKlUPUbEB0dS1GqMy9chxylj1H50N8mJ0Rx7BaUZ1REgSFglBGTCEY4buzY9MpHxCiL0ZDQf4OxPFCcKbi1bch1gTnTMsv2fgcnVNM3-Gjv5AzELXCYEqEZW2LZgEd3FLKxdYRa5CV1sG9Y2qUFEzvRX2UpDQIRc2Na-x4JJ4-lX2ZE352GximCP0A7wMrJrKK5aFcRSf6k8QiBC96PzfG8cPyBJSlANtEo7keXdDBno1lEA6bdSjms_54SQWtHYz2CGnEWULvT4FYIfPj6R3V91ygpmerkKPSpeOm7KorAbZvm3Rb7C1dm2HUb3lleFSyW2pk1FE6dCdoanhAR4SYOVEOcpbW1pwmhmWx0jmc1azcfp5KWntVtY-7MDCf1VgCfzAPfR-4JBGfFP5FlrkgXxJIobq0pmOlTmGbRKsa2cOjpZBbGpAJlH4BaZab8MwXxx_T8soBEUSIK5d9dIcdfVlF4pJ6oImXkl8GycsfqvHbTjg3RboCtLgXYsfaUR06ldEFZeHvhZEDSDmHAQkWNL-5NGOkpHK1N86kdaC_1T9pstjiGvQysecvsZX3wdLCBzcC1NfTjkfnWxXFoc91PGfuIiXpGiIrm1HTIhnC2LEc4A56R7y-WT3Gu4yUpGgIR9wR0t2DSPV130rVDwo6mIDHciroqrg-KtPY1mmwx-5IxVYkXR7YdmVkWrTvJA1PsiiIZRiYGlTY2b2o3W7ltZ3ErXkn_Ap8tdFunzIhKb0Y_3iD-2i9mxvBgCF1wW_UR9IYj-hTz7DiM37rr1O1ReLkfjM0-viHaClYKwW3twoUAl2-pdvEw6gxjI66wgn6faQybw1RoKOkbNgeMeMPHyvDFS6wYpX2l6vUTJY79H9flXycW5c-8ji_hLYSWzIWOqEnaQZbO92GuHeL9O_ijv8lR6wSlRJyW8dJ7a-FnsPR9dfkUU78SYGEfFR-YTaZyRDFbxMyVxktPACAp7IcX-ssGwD_SdQA7wCvLmgjcs2OcxHmXQD2mJmVE3KcFSjKg-rTQz0vuLfV5tm00)