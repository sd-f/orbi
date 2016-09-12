package foundation.softwaredesign.orbi.service;

import foundation.softwaredesign.orbi.model.GameObject;
import foundation.softwaredesign.orbi.model.GeoPosition;
import foundation.softwaredesign.orbi.model.Position;
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
    @Inject
    WorldAdapterService adapterService;
    @Inject
    UserService userService;

    public List<GameObject> getObjectAround(GeoPosition geoPosition) {
        GeoPosition north = adapterService.toGeo(new Position(0d,0d,128d), geoPosition);
        GeoPosition south = adapterService.toGeo(new Position(0d,0d,-128d), geoPosition);
        GeoPosition west = adapterService.toGeo(new Position(-128d,0d,0d), geoPosition);
        GeoPosition east = adapterService.toGeo(new Position(128d,0d,0d), geoPosition);
        return gameObjectRepository.findGameObjectsAround(north.getLatitude(),south.getLatitude(),west.getLongitude(),east.getLongitude());
    }

    public void delete(Long id) {
        GameObject gameObject = gameObjectRepository.findBy(id);
        if (Objects.isNull(gameObject)) {
            throw new NotFoundException();
        }
        gameObjectRepository.remove(gameObject);
    }

    public void save(GameObject gameObject) {
        gameObject.setIdentityId(userService.getIdentity().getId());
        gameObjectRepository.save(gameObject);
    }

    public void deleteAll() {
        gameObjectRepository.deleteAll();
    }
}
