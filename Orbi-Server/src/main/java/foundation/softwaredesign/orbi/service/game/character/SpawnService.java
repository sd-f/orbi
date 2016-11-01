package foundation.softwaredesign.orbi.service.game.character;

import foundation.softwaredesign.orbi.model.game.character.Character;
import foundation.softwaredesign.orbi.model.game.gameobject.GameObject;
import foundation.softwaredesign.orbi.model.game.gameobject.GameObjectType;
import foundation.softwaredesign.orbi.model.game.transform.GeoPosition;
import foundation.softwaredesign.orbi.model.game.transform.Position;
import foundation.softwaredesign.orbi.model.game.transform.Transform;
import foundation.softwaredesign.orbi.persistence.entity.BodyConstraints;
import foundation.softwaredesign.orbi.service.auth.UserService;
import foundation.softwaredesign.orbi.service.game.gameobject.GameObjectService;
import foundation.softwaredesign.orbi.service.game.gameobject.GameObjectTypeService;
import foundation.softwaredesign.orbi.service.game.gameobject.ai.AiService;
import foundation.softwaredesign.orbi.service.game.world.WorldAdapterService;
import foundation.softwaredesign.orbi.service.game.world.WorldService;

import javax.enterprise.context.RequestScoped;
import javax.inject.Inject;
import java.util.ArrayList;
import java.util.Calendar;
import java.util.Date;
import java.util.List;
import java.util.concurrent.ThreadLocalRandom;
import java.util.stream.Collectors;

import static java.util.Objects.isNull;

/**
 * @author Lucas Reeh <lr86gm@gmail.com>
 */
@RequestScoped
public class SpawnService {

    private static Integer MAX_GIFTS_AROUND = 2;
    private static Integer MAX_NEW_NPCS_AROUND = 1;

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
    WorldService worldService;
    @Inject
    InventoryService inventoryService;

    public void spawnObjects(List<GameObject> objects) {
        Character character = characterService.loadCurrent();
        spawnGifts(objects, character);
        spawnNpcObjects(objects, character);
    }

    private void spawnNpcObjects(List<GameObject> objects, Character character) {
        if (shouldNpcSpawn(character)) {
            if (objects.stream().filter(gameObject -> gameObjectTypeService.isGiftObject(gameObject.getType())).count() <= MAX_GIFTS_AROUND) {
                GameObject gift = newObject(getRandomNpcPrefab(),"NPC_spawned_by_"+ character.getName() );
                setRandomSpawnPoint(gift, character);
                characterService.gifted();
                worldService.create(gift);
            }
        }
    }

    private void spawnGifts(List<GameObject> objects, Character character) {
        if (deservesGift(character)) {
            if (objects.stream().filter(gameObject -> gameObjectTypeService.isGiftObject(gameObject.getType())).count() <= MAX_GIFTS_AROUND) {
                GameObject gift = newObject(getRandomGiftPrefab(),"Gift_for_"+ character.getName());
                setRandomSpawnPoint(gift, character);
                // TODO let gift look at player
                characterService.gifted();
                worldService.create(gift);
            }
        }
    }

    private String getRandomGiftPrefab() {
        List<GameObjectType> types = gameObjectTypeService.loadAll()
                .stream()
                .filter(GameObjectType::getGift)
                .collect(Collectors.toList());
        return inventoryService.getRandomTypeByRarity(types).getPrefab();
    }

    private String getRandomNpcPrefab() {
        List<GameObjectType> types = gameObjectTypeService.loadAllCraftable()
                .stream()
                .filter(gameObjectType -> (gameObjectType.getAi() && !gameObjectType.getGift()))
                .collect(Collectors.toList());
        return inventoryService.getRandomTypeByRarity(types).getPrefab();
    }

    private void setRandomSpawnPoint(GameObject object, Character character) {
        Integer randomX = ThreadLocalRandom.current().nextInt(-25,25);
        Integer randomZ = ThreadLocalRandom.current().nextInt(-25,25);
        GeoPosition giftPosition = worldAdapterService.toGeo(new Position(new Double(randomX),0.00001d,new Double(randomZ)),
                character.getTransform().getGeoPosition());
        object.getTransform().setGeoPosition(giftPosition);
        object.getTransform().getRotation().setY((object.getTransform().getRotation().getY() + 180f)%360f);
        // TODO let gift look at player
        object.setConstraints(BodyConstraints.FREEZE_ROTATION.value());
    }

    private Boolean deservesGift(Character character) {
        return isLastGiftedBefore(character, 15, 120);
    }

    private Boolean shouldNpcSpawn(Character character) {
        return isLastGiftedBefore(character, 300, 1200);
    }

    private Boolean isLastGiftedBefore(Character character, Integer rangeFromMinutes, Integer rangeToMinutes) {
        Calendar cal = Calendar.getInstance();
        Integer randomMinutes = ThreadLocalRandom.current().nextInt(rangeFromMinutes,rangeToMinutes);
        cal.add(Calendar.MINUTE, -randomMinutes);
        Date lastGifted = character.getGiftedOn();
        if (isNull(lastGifted)) {
            Calendar calTmp = Calendar.getInstance();
            calTmp.add(Calendar.HOUR, -7);
            lastGifted = calTmp.getTime();
        }
        return lastGifted.before(cal.getTime());
    }

    private GameObject newObject(String prefab, String name) {
        GameObject newObject = new GameObject();
        newObject.setIdentityId(userService.getIdentity().getId());
        newObject.setType(gameObjectTypeService.loadByPrefab(prefab));
        newObject.setName(name);
        return newObject;
    }
}
