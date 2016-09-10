package foundation.softwaredesign.orbi.service;

import foundation.softwaredesign.orbi.model.GameObject;
import foundation.softwaredesign.orbi.model.GeoPosition;
import foundation.softwaredesign.orbi.model.Player;
import foundation.softwaredesign.orbi.model.World;

import javax.enterprise.context.RequestScoped;
import javax.inject.Inject;
import java.util.List;

/**
 * @author Lucas Reeh <lr86gm@gmail.com>
 */
@RequestScoped
public class WorldService {

    @Inject
    GameObjectService gameObjectService;
    @Inject
    ElevationService elevationService;


    public World getWorld(GeoPosition geoPosition) {
        World world = new World();
        List<GameObject> gameObjectList = gameObjectService.getObjectAround(geoPosition);
        world.setGameObjects(gameObjectList);
        //elevationService.addAltitude(world);
        return world;
    }

    public World delete(Player player) {

        gameObjectService.delete(player.getSelectedObjectId());
        return getWorld(player.getGeoPosition());
    }

}
