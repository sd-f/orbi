package foundation.softwaredesign.orbi.service;

import foundation.softwaredesign.orbi.model.GameObject;
import foundation.softwaredesign.orbi.model.GeoPosition;
import foundation.softwaredesign.orbi.model.Position;
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
    GameObjectRepository repository;
    @Inject
    WorldAdapterService worldAdapter;

    public List<GameObject> getObjectAround(GeoPosition geoPosition) {
        GeoPosition north = worldAdapter.toGeo(new Position(0d,0d,128d), geoPosition);
        GeoPosition south = worldAdapter.toGeo(new Position(0d,0d,-128d), geoPosition);
        GeoPosition west = worldAdapter.toGeo(new Position(-128d,0d,0d), geoPosition);
        GeoPosition east = worldAdapter.toGeo(new Position(128d,0d,0d), geoPosition);
        return repository.findGameObjectsAround(north.getLatitude(),south.getLatitude(),west.getLongitude(),east.getLongitude());
    }

    public void delete(Long id) {
        GameObject gameObject = repository.findBy(id);
        if (Objects.isNull(gameObject)) {
            throw new NotFoundException();
        }
        repository.remove(gameObject);
    }

    public void save(GameObject gameObject) {
        repository.save(gameObject);
    }

    public void deleteAll() {
        repository.deleteAll();
    }
}
