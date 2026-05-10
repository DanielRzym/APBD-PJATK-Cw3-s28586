# APBD - Ćwiczenie 3: Web API 

Aplikacja ASP.NET Core Web API oparta na kontrolerach, symulująca backend dla centrum szkoleniowego. Służy do zarządzania listą sal dydaktycznych oraz ich rezerwacjami. Projekt został przygotowany jako zadanie z przedmiotu Aplikacje Baz Danych.

## Funkcjonalności

*   **Zarządzanie salami (Rooms):** Pobieranie listy sal, wyszukiwanie po identyfikatorze, filtrowanie po budynku (z wykorzystaniem *fall-through* w instrukcji switch) i atrybutach (pojemność, rzutnik, aktywność), dodawanie, aktualizacja oraz usuwanie sal.
*   **Zarządzanie rezerwacjami (Reservations):** Pobieranie rezerwacji (z filtrowaniem po dacie, statusie i sali), dodawanie, aktualizowanie i usuwanie.
*   **Logika biznesowa:** 
    * Blokada rezerwacji nieistniejących lub nieaktywnych sal.
    * Zapobieganie konfliktom czasowym (nakładające się rezerwacje zwracają błąd `409 Conflict`).
    * Blokada usunięcia sali, do której przypisane są rezerwacje.
*   **Walidacja danych:** Wbudowane Data Annotations sprawdzające m.in. wymagane pola, poprawne wartości liczbowe oraz czy czas zakończenia rezerwacji następuje po czasie rozpoczęcia (`IValidatableObject`).
*   **Dane w pamięci:** Aplikacja działa bez zewnętrznej bazy danych (In-Memory `DataStore`), inicjalizując dane startowe przy każdym uruchomieniu.

## Struktura projektu

Projekt zachowuje podział na podstawowe warstwy odpowiedzialności:

```text
WebApiTraining/
├── Controllers/
│   ├── RoomsController.cs        # Punkty końcowe dla /api/rooms
│   └── ReservationsController.cs # Punkty końcowe dla /api/reservations
├── Models/
│   ├── Room.cs                   # Model sali z regułami walidacji
│   └── Reservation.cs            # Model rezerwacji z niestandardową walidacją czasu
├── Data/
│   └── DataStore.cs              # Statyczny magazyn danych (symulacja bazy)
└── Program.cs                    # Konfiguracja potoku HTTP i wstrzykiwania zależności

**Jak uruchomić projekt**

    Wymagania: Zainstalowany .NET SDK (wersja użyta w projekcie, domyślnie 8.0) oraz wybrane IDE (np. JetBrains Rider, Visual Studio) lub terminal.

    Klonowanie: Pobierz repozytorium na swój komputer:
git clone [https://github.com/TWOJA_NAZWA_UZYTKOWNIKA/APBD-PJATK-Cw3-s28586.git](https://github.com/TWOJA_NAZWA_UZYTKOWNIKA/APBD-PJATK-Cw3-s28586.git)
