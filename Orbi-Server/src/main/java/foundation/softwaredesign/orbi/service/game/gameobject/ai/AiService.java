package foundation.softwaredesign.orbi.service.game.gameobject.ai;

import foundation.softwaredesign.orbi.model.game.gameobject.GameObject;
import foundation.softwaredesign.orbi.model.game.gameobject.GameObjectType;
import foundation.softwaredesign.orbi.model.game.gameobject.ai.AiProperties;
import foundation.softwaredesign.orbi.model.game.transform.GeoPosition;
import foundation.softwaredesign.orbi.model.game.transform.Position;
import foundation.softwaredesign.orbi.model.game.transform.Transform;
import foundation.softwaredesign.orbi.service.game.gameobject.GameObjectService;
import foundation.softwaredesign.orbi.service.game.server.DateComparator;
import foundation.softwaredesign.orbi.service.game.server.DateConverter;
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
    @Inject
    DateConverter date;

    public void updateAiTargets(List<GameObject> gameObjectList) {
        List<GameObject> aiGameObjects = gameObjectList
                .stream()
                .filter(gameObject -> {
                    GameObjectType type = gameObject.getType();
                    if (nonNull(type))
                        if (nonNull(type.getAi()))
                            return type.getAi();
                    return false;
                })
                .collect(Collectors.toList());
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


            if (wantsNewTarget(properties, aiGameObject)) {
                properties.setLastTargetUpdate(date.toString(new Date()));
                setNewRandomAiTarget(aiGameObject);
            }

            gameObjectService.saveAndRefresh(aiGameObject);
        }
    }

    private Boolean wantsNewTarget(AiProperties properties, GameObject object) {
        if (isNull(properties.getLastTargetUpdate())) {
            return true;
        }
        if (DateComparator.isTimeOlderThan(Calendar.SECOND, ThreadLocalRandom.current().nextInt(45, 90), date.toDate(properties.getLastTargetUpdate()))) {
            return true;
        }
        if (nonNull(properties.getTarget())) {
            Double distance = properties.getTarget().getGeoPosition().distanceTo(object.getTransform().getGeoPosition());
            if (isNull(distance)) {
                return true;
            }
            if (distance <= 0) {
                return true;
            }
        }
        return false;
    }

    private void setNewRandomAiTarget(GameObject object) {
        Double randomX = ThreadLocalRandom.current().nextDouble(-25d, 25d);
        Double randomZ = ThreadLocalRandom.current().nextDouble(-25d, 25d);
        Position position = new Position(randomX, 0.000001d, randomZ);
        GeoPosition geoPosition = worldAdapterService.toGeo(position, object.getTransform().getGeoPosition());
        object.getAiProperties().getTarget().setGeoPosition(geoPosition);
    }
}
