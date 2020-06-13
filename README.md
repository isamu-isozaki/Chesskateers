## Chesskateers

Read the wiki for User Guide here's the system manual.
# Minimum hardware requirements
8 GB of RAM and 2 GHz of CPU
# Build instruction
1. Install heroku client and Unity.
2. Open unity from the chesskateers folder. 
3. Build unity with webgl into the Server/chesskateers/public/UnityExport folder
4. Go to the server folder and type cd chesskateers && npm run build && cd .. && heroku local to run the server locally

# Known issues
1. Sometimes players can't be played with. ->
Can be fixed by 1. reloading the page or 2. Signing out and signing back in.
2. The black piece sometimes facing the wrong way -> 
Cannot be fixed currently
3. The cards sometimes duplicates ->
Cannot be fixed currently

# Contact
Contact imi25@drexel.edu if any issues in the game shows up.