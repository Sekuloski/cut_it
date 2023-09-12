import json
from django.http import HttpResponse, JsonResponse
from rest_framework.decorators import api_view
from rest_framework.response import Response
from .serializers import PlayerSerializer
from .models import Player
import logging
from systemd.journal import JournalHandler
log = logging.getLogger('demo')
log.addHandler(JournalHandler())
log.setLevel(logging.DEBUG)

@api_view(['POST'])
def update_player(request):
    """
    /update

    Update a players score. If the player doesn't exist, he will be created

    {
        "username": "Sekuloski",
        "high_score": 200,
        "location": "41.99615797025478,21.39755421534261"
    }
    """
    data = json.loads(request.body)

    try:
        username = data['name']
        high_score = data['high_score']
        location = data['location']
    except KeyError as e:
        log.info(data)
        log.info(e)
        return HttpResponse(e, status=401)

    if Player.objects.filter(name=username).exists():
        player = Player.objects.get(name=username)
        player.high_score = high_score
        player.location = location
        player.save()

    else:
        Player(name=username, high_score=high_score, location=location).save()

    return HttpResponse('Success!', status=200)


@api_view(['GET'])
def get_scores(request):
    """
    /players

    Get all Players and their scores.
    """
    players = Player.objects.all()
    serializer = PlayerSerializer(players, many=True)

    return JsonResponse({'players': serializer.data})
