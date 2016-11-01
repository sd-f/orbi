package foundation.softwaredesign.orbi.service.game.world;

import foundation.softwaredesign.orbi.model.game.character.Character;
import foundation.softwaredesign.orbi.model.game.gameobject.GameObject;
import foundation.softwaredesign.orbi.model.game.transform.GeoPosition;
import foundation.softwaredesign.orbi.model.game.world.World;
import foundation.softwaredesign.orbi.persistence.entity.BodyConstraints;
import foundation.softwaredesign.orbi.service.auth.UserService;
import foundation.softwaredesign.orbi.service.game.character.CharacterService;
import foundation.softwaredesign.orbi.service.game.character.SpawnService;
import foundation.softwaredesign.orbi.service.game.gameobject.GameObjectService;
import foundation.softwaredesign.orbi.service.game.gameobject.GameObjectTypeService;
import foundation.softwaredesign.orbi.service.game.gameobject.ai.AiService;

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
    AiService aiService;
    @Inject
    GameObjectTypeService gameObjectTypeService;
    @Inject
    CharacterService characterService;
    @Inject
    UserService userService;
    @Inject
    WorldAdapterService worldAdapterService;
    @Inject
    SpawnService spawnService;

    public World getWorld(GeoPosition geoPosition) {
        World world = new World();
        List<GameObject> gameObjectList = gameObjectService.getObjectAround(geoPosition);
        world.setGameObjects(gameObjectList);
        //elevationService.addAltitude(world);
        spawnService.spawnObjects(gameObjectList);
        List<Character> characterList = characterService.getCharactersAround(geoPosition);

        aiService.updateAiTargets(gameObjectList);
        world.setCharacters(characterList);
        return world;
    }

    public void create(GameObject gameObject) {
        if (gameObject.getType().getAi())
            gameObject.setConstraints(BodyConstraints.FREEZE_ROTATION.value());
        gameObject.setCreateDate(new Date());
        gameObjectService.save(gameObject);
    }

    public void delete(Long gameObjectId) {
        gameObjectService.delete(gameObjectId);
    }

}
