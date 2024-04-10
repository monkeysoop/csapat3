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
![UML class diagram](https://www.plantuml.com/plantuml/svg/pLXVS-Es4N_Nfy38BxHDPv8-TZpds98lfndVsPKqSTkRcnkOtADGam0BWB8LLVdinP8KiQ8WtNNIDl63HM9_MFoms3zB_6WirxOgSZRAkJ5ZmJFDYu5v5hBaxeabE7njhHPFbOL9ekHC_0Bi-y5WKFBacMV0XZyBU1cozO0nrk3PTLdYU_jtCPkCR-zkF_mqVyV-p59LFUN0rawLiHiF-kiToCmk7498wuV_5XwUMIYRCRRMm5CbynLHzjc8ub78LBquq4mKLSwjKB8PkY8u3MNELnrqIbufh3QJL-ZVA6ZfxF1UfP0xoC_RPmf8hCx7waKw-PDIEP4KgZBme8oJF2YwKMIOM2vpSA99mJEuqPLP43dakIWDuDJRvl6CpbPFog9iB1Ah9DUhooaEnUmdBT8t16cvpk1ReEVkTAFJxPlNS2zhkrHbP42ce9qczVGFI9m29DyARvSWxRNEpAcVsLZzfh9ksS-ZN2JFOHLJLLb8GnhS4V-MwMDXIcM0d6j4fFSbo8PYTEgFZWZTwlkIjR7SYcJJQ7UJxnHFXSmYzA1c4RB6YES9EYb4YHEFk5poNGkbck-G6hjtF9SmVYA5hG_FXEKu-pQ70gGrhTODYTz-nB0rbT8WGSbcC13IWlx24s2JaIeABbCKaD4FoeelgmUjIj1sDLfmcG5YI1hfGWPpIRrSDtB33R3BVrrS-A5TG2Fqj8_26luUGhOsXXmoRg7R10sqLqWgYuvqdU0QburlNRq9Pw24ozjkneE6L2HESJVfoiVJg4EsTd4JQYzzvoZae0cqq_CrdRsDH0V9n-nhH4z3JAEwF80bGNQr5mBdny0UDUN2m9uHEhSanzDpY4l-PApcYNtZpi3hwtcPMxCtSi2uzxA7Yyhs_GClq3XkCI6pblecn8lGE1afQRNAythN_eMuo2zA5Lr5z3mUHMeNXulR-JiGsS8U0Vmzh7UwhGNrmrbmwYFFSuJWRnWnT_dri8DYaZlYm2Vtia7qCpGQrMDmZG3YS9eA7S05okaX47pWECu3sgAIsqLty9G0pOJ1nPs8hjr2oL38jgDAQv1sBWheDJpdpVnWCtIE-3l-17b7bGI3c-M6jscP27Xb82ZfxIfi19gv7q0ROIp817RxGBVTBoAZPYbu-LRaFfYyn6cIze3C0Y-mx-ntWPQRvmDJP5NCdTHCOGay3qsyDaPa4alWp6fk8Llrr90YtZXspnBkgTdWVgAwKLplL8_vggGi0w4UYcyw-fwNU5hkvxwEXu_nfs-bsT3ngkn2c8-OF5vZXZ-hJz7fZcwsnhp7hT8RulDaVtSgGxlJ7TO1Rr1D_NH695ehZPBwWGYSRqe96jtNTFcAo4iDIw4gb8sv1SJHJ9FL-YBzGypqdsBMMYncXScekz89o2m2_DyH9OWPNqB8su-RE1mJFfkua40nTG-I1WuKiPFFUZTR4yaBoT5a7Cs3cGP_QPMfkx0bprlHhEIIA7j2ro0kyjjgq1-gs9nxCqU1ycAv1TgyhBS7iVdFojN8iGyNBC-aQIXc9Q0g_D-v-twN4oWLL1oiOc4tmjD7HtqhMcAJ9kU5gXxLTYW6BaBPPStDIYQRNL1zijzaTBK4hKKA6sg7u1Q7FmpPcMlVXnUVURkD8TaHiT23t_C1k_pQVMyXDje9WfZLN_lRJ_6OfK93WcIs2GPakfLkUrxssZRMPtiPusBkbc_MARd69Dde-VWfPYy-pEYoeLNwwazmX_yrzWqX6WFzRPoyvtB1hGKknyADoGIYuupTJqZBwxpIqapXdvLZbaO7wHvNsukPyTiPM_-FRKDTjL_lVz_bhr6n9-1TxntmSY8DQ1ije6uWyE8CFIyddMJCX8MY5Pw6jKoXS0IEQJbjntyFCtGt3JuXq2z2l0w9RDE2-Oo2FLdCUjnwPJTcRKKtoAXvF2E97JUy-wNQh4_p5Kt2GvASUl3ll_lkRCXKCwEXCMnOaAFsU9ha2SU_L531PSkKtE_XijlBFR5pkuSI0knt1Fyl6te6pHOZlLKGA95ONtO--PKHD6aSR3UYrnOIqDgi5WgZoNYaAdInVbC-IO-qG3Fgvdr4aBQNZ4bCTCzUZ9ystfXNf2yJnYgzylNYS7CMbMHr4edzLVidnJlbH7ATfiWfAhVAhgXz6ylsN7pHK1ssokX1jFjYwwt-7PFPhnzrgy3JeMn8bx0JHKAsfqdjh3CuVeOJSxy4tQQf4EBN3_1g8r_2lQ3x3XEVEYdzz2AKaUok_V9Tmu6iYdQ0REfjpAOn6yNiCMPpgW_X_eZ93AcqNSGCkC6EH5NuyWB63lQYudoKg-JvKHZ0pL5bRhyWZNlS12OrWVIjS_6ZYdnLpS5WLbm7s-5Q6Bgg3WXkbEr6tGhNPObctRluxWJqpdimw2wvyJwvVsay25zsDzYT6yp1e7-15D0sw7Fg4UqcjyewKE2Ys-TjPEDWachoqY2mqKP0WwxMDdlphJXfm562tTdUs4rW3FiOD_r7a6bLvFy6)