package foundation.softwaredesign.orbi.service;

import foundation.softwaredesign.orbi.model.GameObject;
import foundation.softwaredesign.orbi.model.GeoPosition;
import foundation.softwaredesign.orbi.persistence.entity.GameObjectEntity;
import foundation.softwaredesign.orbi.persistence.repo.GameObjectRepository;

import javax.enterprise.context.RequestScoped;
import javax.inject.Inject;
import javax.ws.rs.NotFoundException;
import java.util.List;
import java.util.Objects;

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

    public void delete(Long id) {
        GameObject gameObject = gameObjectRepository.findBy(id);
        if (Objects.isNull(gameObject)) {
            throw new NotFoundException();
        }
        gameObjectRepository.remove(gameObject);
    }
}
