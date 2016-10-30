package foundation.softwaredesign.orbi.service.game.gameobject.ai;

import foundation.softwaredesign.orbi.model.game.character.CharacterDevelopment;
import foundation.softwaredesign.orbi.model.game.gameobject.GameObject;
import foundation.softwaredesign.orbi.model.game.gameobject.ai.AiProperties;
import foundation.softwaredesign.orbi.model.game.transform.GeoPosition;
import foundation.softwaredesign.orbi.model.game.transform.Position;
import foundation.softwaredesign.orbi.model.game.transform.Transform;
import foundation.softwaredesign.orbi.service.game.gameobject.GameObjectService;
import foundation.softwaredesign.orbi.service.game.server.DateComparator;
import foundation.softwaredesign.orbi.service.game.world.WorldAdapterService;

import javax.enterprise.context.RequestScoped;
import javax.inject.Inject;
import java.util.Calendar;
import java.util.Date;
import java.util.List;
import java.util.concurrent.ThreadLocalRandom;
import java.util.stream.Collectors;

import static java.util.Objects.isNull;
import static java.util.Objects.nonNull;

/**
 * @author Lucas Reeh <lr86gm@gmail.com>
 */
@RequestScoped
public class AiService {

    @Inject
    GameObjectService gameObjectService;
    @Inject
    WorldAdapterService worldAdapterService;

    public void updateAiTargets(List<GameObject> gameObjectList) {
        List<GameObject> aiGameObjects = gameObjectList.stream().filter(gameObject -> gameObject.getType().getAi()).collect(Collectors.toList());
        for (GameObject aiGameObject: aiGameObjects) {
            AiProperties properties = aiGameObject.getAiProperties();
            if (isNull(properties)) {
                properties = new AiProperties();
                aiGameObject.setAiProperties(properties);
            }
            if (isNull(properties.getTarget())) {
                properties.setTarget(new Transform());
                properties.getTarget().setGeoPosition(aiGameObject.getTransform().getGeoPosition());
            }


            if (isNull(properties.getLastTargetUpdate())
                    || (nonNull(properties.getLastTargetUpdate())
                    && DateComparator.isTimeOlderThan(Calendar.SECOND, -30, properties.getLastTargetUpdate()))) {
                properties.setLastTargetUpdate(new Date());
                setNewRandomAiTarget(aiGameObject);
            }

            gameObjectService.saveAndRefresh(aiGameObject);
        }
    }

    private void setNewRandomAiTarget(GameObject object) {
        Double randomX = ThreadLocalRandom.current().nextDouble(-25d, 25d);
        Double randomZ = ThreadLocalRandom.current().nextDouble(-25d, 25d);
        Position position = new Position(randomX, 0.0d, randomZ);
        GeoPosition geoPosition = worldAdapterService.toGeo(position, object.getTransform().getGeoPosition());
        object.getAiProperties().getTarget().setGeoPosition(geoPosition);
    }
}
