package foundation.softwaredesign.orbi.service.game.world;

import foundation.softwaredesign.orbi.model.game.gameobject.GameObject;
import foundation.softwaredesign.orbi.model.game.transform.GeoPosition;
import foundation.softwaredesign.orbi.model.game.world.World;
import foundation.softwaredesign.orbi.persistence.repo.game.world.ElevationRepository;

import javax.enterprise.context.RequestScoped;
import javax.inject.Inject;

import static java.util.Objects.nonNull;

/**
 * @author Lucas Reeh <lr86gm@gmail.com>
 */
@RequestScoped
public class ElevationService {

    @Inject
    ElevationRepository elevationRepository;

    public void addAltitude(World world) {
        for (GameObject gameObject : world.getGameObjects()) {
            addAltitude(gameObject.getTransform().getGeoPosition());
        }
    }

    public void addAltitude(GeoPosition position) {
        Double elevation = getAltitude(position);
        if (nonNull(elevation)) {
            position.setAltitude(elevation);
        }
    }

    public Double getAltitude(GeoPosition position) {
        return elevationRepository.getAltitude(position.getLatitude(), position.getLongitude());
    }
}
