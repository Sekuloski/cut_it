# Cut It

#### Bojan Sekuloski 191263

## Overview

Cut It is a hyper-casual game made in Unity using C#. It’s a very simple game where the
player cuts planks until he fills the screen, at which point the game ends. The goal is to cut the
planks perfectly in the middle so the planks disappear. The game also communicates with a
back end to keep track of each player, their location and their highscore.

## The Game

### Functionality

As stated previously, the goal of the game is to last as long as possible by cutting the
plank in half every time. If the plank is cut wrong, the pieces fall down and start to gather in a
pile. There is a line in the middle of the screen that indicates the point to which the planks can
collect. If a plank crosses the line for more than 3 seconds, the game ends and the player has
an option to continue or end the game.

### Axes

The player also has a variety of options for which axe to use. The goal is for each axe to
be locked and purchasable as the player plays the game. In the current iteration of the game, all
axes are unlocked. These do not provide the player with any benefit other than design. Some
axes would have special effects while cutting the plank in the middle, rewarding the player for
each perfect cut.

## Architecture

### The Game

The game is made in Unity using C#. Unity is a standard game development software for
small startups (until the 1st of January when they will implement a new pricing plan). C# in this
case is used as the main scripting language of the game. The game has a couple of scripts
controlling everything, from the UI, to the main functionality, to the connection to the outer APIs.

### Functionality

#### Plank.cs

The main game works by moving a single plank in the middle of the screen left and right
from one end to the other. This is achieved by the *Plank.cs* script. It has an *Update()* method,
which is a Unity standard, that is called on each frame. If the game is in the *isPlaying* state, this
method calls the *MovePlank()* function in *Plank.cs*, that moves the plank left and right. This is
achieved by a boolean indicating the current direction and an if else statement that checks how
far left or right the object is. This script is attached to the plank in the middle of the screen, so it
is never instantiated twice. This script also has methods to reset the plank position and a
method to “cut” the object, by giving another script its position and movement speed.

#### Axe.cs

This is the script that actually “cuts” the plank. This script is attached to the axe in the
game, and calls the *Cut()* method when it interacts with the plank, by colliding with it. This
method essentially makes the plank disappear for a slight delay, indicated by a variable
*respawnTime.* This is only called if the *Plank.Cut()* method returns true, indicating that the plank
is in a position where it can be “cut”. After *respawnTime* passes, the plank is made visible again.

#### CutPlankSpawner.cs

This is the script that actually “cuts” the plank. Cuts in quotes, because we don’t actually
cut the plank. The script also has a method called *Cut(),* which when called, checks the planks
current position and decides whether or not it can be “cut”. This depends on the planks current
position and whether it is colliding with a vertical line in the middle, the “Cut Line”. If it is, it
checks where the plank collides with the line, indicating how the plank should be “cut”. Finally,
the script spawns two new objects, using the *Factory Pattern,* which are either two halves, or a
combination of ¼, ⅛, or ⅓. After that, the script updates the score by calling the *Game*
*Manager,* which is another script.

#### GameManager.cs

This is the main script of the game. This controls the current state, the score and the
controls of the UI, regarding the game. The *isPlaying* variable is declared here and is public for
each of the other scripts to be able to see. The score is also handled by this script, by a method
*UpdateScore(int points)* which is called by the previous script. The scores are handled by this
script by saving the highscore to local storage, using *PlayerPrefs.* If the current score is higher
than the highest score, the highest score is updated in local storage and in the current game.
This is done using the methods *UpdateScore(int points), UpdateHighScore(), LoadHighScore().*
Here the username is also loaded, using *LoadUsername(),* also stored in local storage. Finally,
the UI elements are controlled here, by animating their opacity and disabling/enabling the
buttons on screen, based on the current state of the game, if we are in the menu, in the game,
or in the post-game screen.

#### MidLineManager.cs and PieceCollider.cs

These two scripts are the final scripts controlling the game functionality. The first is the
script attached to the horizontal line on the screen that controls its color based on the highest
cut piece in the pile of planks on the bottom. The line gets more red the higher the highest plank
is. The other script, *PieceCollider.cs,* is attached to each piece on the ground. This script has
two Unity methods called *OnTriggerEnter2D(Collder2D collision), OnTriggerExit2D(Collider2D*
*collision).* Both are self-explanatory. The first is called when this object interacts with any other
object, while the second is called when this interaction is ended. These two methods control a
so-called coroutine, which is a method that is called based on a certain interval.
*OnTriggerEnter2D* starts this coroutine, called *PlankCountdown()*, while the other method stops
it. The goal is two count to three, to decide if the game should end. Basically, as long as
this piece is colliding with the middle line, the script counts to three. Once it reaches 3, the game
ends. At this point the player can choose to continue the game, by clearing the screen of all the
pieces, or end it.

### UI

The UI is controlled by using Unity's Animator, and the scripts *GameManager.cs* and
*AxeSelector.cs.* The former controls which buttons are visible on the screen. If we are in the
main menu, the start buttons are shown, while if we are post-game, the restart and end buttons
are shown. This script also animates these by fading their opacity. The latter script controls
which axe is currently selected.

### Software Design Patterns

#### Singleton Pattern

The singleton pattern is used to ensure that only one plank is moving in the middle at a
time. This object is only created once and is never destroyed and recreated. The same goes for
the GameManager, MidLineManager and the Axe.

#### Factory Pattern

The factory pattern is used to create the objects once the main plank is cut. The objects
that are created depend on the position of the plank relative to the vertical line in the middle.

#### Observer Pattern

The observer pattern is used by the CutPlankSpawner script, which observes the
position of the main plank, as well as the GameManager script, which observes multiple game
states from multiple scripts.

### Back End

This project also has a back-end, which is a simple Django project hosted on
<http://sekuloski.mk:25565>. This serves as the main service that keeps track of all players, their
location and their highscore. This is handled using a script called *API.cs*. This script has
methods called *SendGetRequest(string url) and SendPostRequest(string url, string data).* These
methods use Unity’s UnityWebRequest objects to call the back-end. There are 2 main endpoints
used:

#### /players

This is a GET endpoint, which returns all players and their highscore and location:

    {
        "players":[
            {
                "id":1,
                "name":"Sekuloski",
                "high_score":58,
                "location":"MK"
            },
            {
                "id":2,
                "name":"Test Player",
                "high_score":345,
                "location":"FR"
            },
            {
                "id":3,
                "name":"Test Player 2",
                "high_score":300,
                "location":"US"
            }
        ]
    }

#### /update

This is a POST endpoint that is used to update the player’s score using the following
body:

    {
        "name":"Sekuloski",
        "high_score":58,
        "location":"MK"
    }

### Services

Finally, for the sake of requirements, the following services are implemented, using the
*Services.cs* script:

#### Location

This service is implemented using an API from OpenCageData,

    https://api.opencagedata.com/geocode/v1/json?q={latitude}+{longitude}&key={openCageApiKey}

Here, the latitude and longitude are gathered using the device’s GPS coordinates:

*float latitude = Input.location.lastData.latitude;*

*float longitude = Input.location.lastData.longitude;*

The API key is just a string from the OpenCageData interface. This returns the Country from
which the player is playing in the form of two uppercase letters. The location is also stored just
to keep track of the current coordinates, which are used later to show them in the settings page.

#### Camera

The camera is just a simple WebCamTexture object, which is a texture that is transferred
to an image on the screen. This works only if the device has a camera.


