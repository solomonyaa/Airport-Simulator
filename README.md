Airport Simulator - Project Overview

The Airport Simulator is a real-time system designed to simulate air traffic control operations, including arrivals and departures, through an architectural framework divided into a logical core, remote services, data persistence, a simulation engine, unit testing, and a visual user interface.

Core System Architecture & Layers

●	Logical Layer (Logic & Entities):
○	Manages the airport's overall state and individual operational units.
○	Built around key components: Airplanes/Flights, Stations (representing distinct points or segments along airport paths), and a State Manager that tracks all stations and the planes occupying them.
○	Designed with complete separation of concerns and interface abstraction (e.g., abstracting time-sampling and dependencies like DB/Simulators) to enforce testability and allow robust Unit Testing before implementing external components.
●	Data Layer & Persistence:
○	Uses a database exposed via a Repository pattern to keep track of system state, history, and schedules.
○	Maintains four main database entities: real-time station statuses, operational history (tracking plane entry/exit timestamps per station), scheduled departures, and scheduled arrivals (preserving future queries without deleting historical data).
●	Services & Communication Layer:
○	Exposes the logical layer remotely using WCF (Windows Communication Foundation) services (supporting basic HTTP, duplex, and reliable messaging).
○	Handles Data Transfer Objects (DTOs) configured with proper serialization attributes to push real-time state changes seamlessly to clients.
●	Simulators:
○	Generates incoming landing and outgoing takeoff events, pushing them to the logic layer via services.
○	Can be replaced or preceded by known data sets in unit tests.
●	User Interface (UI):
○	Displays scheduled arrivals and departures, a tabular real-time view of station states, and a graphical layout/grid view representing the airport layout.
○	Automatically loads the current state on startup and reacts to live data feeds pushed from the server.



Operational Rules & Logic

●	Station Flow:
○	Stations are connected in designated routes for arrivals and departures.
○	Each station can hold at most one plane at a time. When a plane finishes its dwell time at a station, it queries subsequent connected stations; if a target station is free, the plane moves forward immediately, otherwise it listens and waits until a target station frees up.
●	Arrival and Departure Routes:
○	Arrivals: Approach path points leading to landing, followed by taxiing to parking/gate areas.
○	Departures: Start from designated stands or gates, taxi toward the runway, perform takeoff, and exit the active airspace.
●	Milestones & Definition of Done (DoD):
○	The project emphasizes a strict testing phase (Step A) verifying various logical permutations (e.g., station occupancy checks, movement blocking when paths are blocked, proper segregation of landing vs. takeoff paths, and persistence triggers) via Unit Tests prior to UI and simulator integration.
