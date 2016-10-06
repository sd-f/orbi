package foundation.softwaredesign.orbi.service;

import foundation.softwaredesign.orbi.model.Character;
import foundation.softwaredesign.orbi.model.GameObject;
import foundation.softwaredesign.orbi.model.GeoPosition;
import foundation.softwaredesign.orbi.model.World;

import javax.enterprise.context.RequestScoped;
import javax.inject.Inject;
import java.util.Date;
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
    @Inject
    CharacterService characterService;

    public World getWorld(GeoPosition geoPosition) {
        World world = new World();
        List<GameObject> gameObjectList = gameObjectService.getObjectAround(geoPosition);
        world.setGameObjects(gameObjectList);
        //elevationService.addAltitude(world);
        List<Character> characterList = characterService.getCharactersAround(geoPosition);
        world.setCharacters(characterList);
        return world;
    }

    public void create(GameObject gameObject) {
        gameObject.setCreateDate(new Date());
        gameObjectService.save(gameObject);
    }

    public void delete(Long gameObjectId) {
        gameObjectService.delete(gameObjectId);
    }

}
