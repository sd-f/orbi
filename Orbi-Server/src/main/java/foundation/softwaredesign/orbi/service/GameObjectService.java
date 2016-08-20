package foundation.softwaredesign.orbi.service;

import foundation.softwaredesign.orbi.model.GameObject;
import foundation.softwaredesign.orbi.model.GeoPosition;
import foundation.softwaredesign.orbi.persistence.repo.GameObjectRepository;

import javax.enterprise.context.RequestScoped;
import javax.inject.Inject;
import java.util.List;

/**
 * @author Lucas Reeh <lr86gm@gmail.com>
 */
@RequestScoped
public class GameObjectService {

    @Inject
    GameObjectRepository gameObjectRepository;

    public List<GameObject> getObjectAround(GeoPosition geoPosition) {
        return gameObjectRepository.findGameObjectsAround(geoPosition.getLatitude(), geoPosition.getLongitude());
    }
}
