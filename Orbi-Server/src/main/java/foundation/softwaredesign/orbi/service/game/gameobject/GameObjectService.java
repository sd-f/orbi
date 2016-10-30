package foundation.softwaredesign.orbi.service.game.gameobject;

import foundation.softwaredesign.orbi.model.game.gameobject.GameObject;
import foundation.softwaredesign.orbi.model.game.transform.GeoPosition;
import foundation.softwaredesign.orbi.model.game.transform.Position;
import foundation.softwaredesign.orbi.persistence.repo.game.gameobject.GameObjectRepository;
import foundation.softwaredesign.orbi.persistence.repo.game.gameobject.GameObjectStatisticsRepository;
import foundation.softwaredesign.orbi.service.game.world.WorldAdapterService;

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
    GameObjectStatisticsRepository statisticsRepository;
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
        GameObject gameObject = this.findById(id);
        repository.remove(gameObject);
    }

    public GameObject findById(Long id) {
        GameObject gameObject = repository.findBy(id);
        if (Objects.isNull(gameObject)) {
            throw new NotFoundException();
        }
        return gameObject;
    }

    public void save(GameObject gameObject) {
        repository.save(gameObject);
    }

    public GameObject saveAndRefresh(GameObject gameObject) {
        return repository.saveAndFlushAndRefresh(gameObject);
    }

    public void deleteAll() {
        repository.deleteAll();
    }

    public Long count() {
        return statisticsRepository.countAllObjects();
    }
}
