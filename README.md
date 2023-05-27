# VR Experience End Project

## Project name: Chased

Group members:

- Boukhada Youssef (yousef12333)
- Cuypers Jan (Studentaccount456 & Janprogramming4 (because of vscode glitch))
- Stoica Marius (MariusSpaceEngineer)
- Van Loon Joppe (Joppe-vL)

### 1. Initialisation Github repository

Unity version: 2021.3.23f1

When we tried to work with the ML-Agents plugin for unity we came accross many issues when starting from the VR room unity project. Thus we will initialise this github repository by all pulling the base project and installing the ML-Agents and VR plugins on our devices before doing a push.

Step plan (to be removed):

1) Vanboven: Window/Package Manager
(Packages: Unity Registry en dan rechts in zoekbalk zoeken)
2) Install: XR interaction Toolkit (Zou 2.3.2 moeten zijn)
(Eerste warning scherm: Bij de native backends warning klik je op yes)
(Tweede warning scherm: CANCEL: staat uitgelegd in dat venster dat het tweede alleen voor old projects is)
3) Install: XR Plugin Management (Zou 4.3.3 moeten zijn)
4) Install: OpenXR Plugin (Zou 1.7.0 moeten zijn)
5) Install: Universal RP (Zou 12.1.11 moeten zijn)
6) Restart unity editor 
7) Scenes => Rename Samplescene to "ML_AgentScene"
8) Scenes => Add scene => Name: "VR_Scene"
9) Vanboven: edit/Project Settings
10) Interaction Profiles => + => Oculus Touch Controller Profile
11) Interaction Profiles => + => Valve Index Controller Profile
12) Vanboven: Window/Package Manager
12.1) XR Interaction Toolkit => Samples => Import
	- Starter Assets
	- XR Device Simulator
13) Install: ML Agents (Zou 2.0.1 moeten zijn)
14) Restart unity and it's done
