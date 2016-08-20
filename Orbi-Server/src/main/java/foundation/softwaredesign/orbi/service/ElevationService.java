package foundation.softwaredesign.orbi.service;

import foundation.softwaredesign.orbi.model.GameObject;
import foundation.softwaredesign.orbi.model.GeoPosition;
import foundation.softwaredesign.orbi.model.World;
import foundation.softwaredesign.orbi.persistence.repo.ElevationRepository;

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
            addAltitude(gameObject.getGeoPosition());
        }
    }

    public void addAltitude(GeoPosition position) {
        Double elevation = getAltitude(position);
        if (nonNull(elevation)) {
            position.setLongitude(elevation);
        }
    }

    public Double getAltitude(GeoPosition position) {
        return elevationRepository.getAltitude(position.getAltitude(), position.getLatitude());
    }
}
