package foundation.softwaredesign.orbi.service.game.world;

import foundation.softwaredesign.orbi.model.game.character.Character;
import foundation.softwaredesign.orbi.model.game.gameobject.GameObject;
import foundation.softwaredesign.orbi.model.game.gameobject.GameObjectType;
import foundation.softwaredesign.orbi.model.game.transform.GeoPosition;
import foundation.softwaredesign.orbi.model.game.transform.Position;
import foundation.softwaredesign.orbi.model.game.world.World;
import foundation.softwaredesign.orbi.persistence.entity.BodyConstraints;
import foundation.softwaredesign.orbi.service.auth.UserService;
import foundation.softwaredesign.orbi.service.game.character.CharacterService;
import foundation.softwaredesign.orbi.service.game.gameobject.GameObjectService;
import foundation.softwaredesign.orbi.service.game.gameobject.GameObjectTypeService;

import javax.enterprise.context.RequestScoped;
import javax.inject.Inject;
import java.util.Calendar;
import java.util.Date;
import java.util.List;
import java.util.concurrent.ThreadLocalRandom;

import static java.util.Objects.isNull;

/**
 * @author Lucas Reeh <lr86gm@gmail.com>
 */
@RequestScoped
public class WorldService {

    private static Integer MAX_GIFTS_AROUND = 2;

    @Inject
    GameObjectService gameObjectService;
    @Inject
    GameObjectTypeService gameObjectTypeService;
    @Inject
    ElevationService elevationService;
    @Inject
    CharacterService characterService;
    @Inject
    UserService userService;
    @Inject
    WorldAdapterService worldAdapterService;

    public World getWorld(GeoPosition geoPosition) {
        World world = new World();
        List<GameObject> gameObjectList = gameObjectService.getObjectAround(geoPosition);
        world.setGameObjects(gameObjectList);
        //elevationService.addAltitude(world);
        createGift(gameObjectList);
        List<Character> characterList = characterService.getCharactersAround(geoPosition);
        world.setCharacters(characterList);
        return world;
    }

    private void createGift(List<GameObject> objects) {
        Character character = characterService.loadCurrent();
        Calendar cal = Calendar.getInstance();
        Integer randomMinutes = ThreadLocalRandom.current().nextInt(15,120);
        cal.add(Calendar.MINUTE, -randomMinutes);
        Date lastGifted = character.getGiftedOn();
        if (isNull(lastGifted)) {
            Calendar calTmp = Calendar.getInstance();
            calTmp.add(Calendar.HOUR, -7);
            lastGifted = calTmp.getTime();
        }
        if (lastGifted.before(cal.getTime())) {
            if (objects.stream().filter(gameObject -> {
                GameObjectType type = gameObject.getType();
                return gameObjectTypeService.isGiftObject(gameObject.getType());
            }).count() <= MAX_GIFTS_AROUND) {
                GameObject gift = new GameObject();
                gift.setIdentityId(userService.getIdentity().getId());
                gift.setType(gameObjectTypeService.loadByPrefab(GameObjectTypeService.GIFT_CHEST_OBJECT_TYPE_PREFAB));
                gift.setName("Gift_For_"+character.getName());

                Integer randomX = ThreadLocalRandom.current().nextInt(-15,30);
                Integer randomZ = ThreadLocalRandom.current().nextInt(-15,30);
                GeoPosition giftPosition = worldAdapterService.toGeo(new Position(new Double(randomX),100d,new Double(randomZ)),
                        character.getTransform().getGeoPosition());
                gift.getTransform().setGeoPosition(giftPosition);
                gift.getTransform().getRotation().setY((gift.getTransform().getRotation().getY() + 180f)%360f);
                gift.setConstraints(BodyConstraints.FREEZE_ROTATION.value());
                // TODO let gift look at player
                characterService.gifted();
                create(gift);
            }
        }
    }


    public void create(GameObject gameObject) {
        gameObject.setCreateDate(new Date());
        gameObjectService.save(gameObject);
    }

    public void delete(Long gameObjectId) {
        gameObjectService.delete(gameObjectId);
    }

}
