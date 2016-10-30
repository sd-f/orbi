package foundation.softwaredesign.orbi.service.game.character;

import foundation.softwaredesign.orbi.model.game.character.CharacterDevelopment;
import foundation.softwaredesign.orbi.model.game.character.Inventory;
import foundation.softwaredesign.orbi.model.game.character.InventoryItem;
import foundation.softwaredesign.orbi.model.game.gameobject.GameObject;
import foundation.softwaredesign.orbi.model.game.gameobject.GameObjectType;
import foundation.softwaredesign.orbi.persistence.entity.GameObjectTypeEntity;
import foundation.softwaredesign.orbi.persistence.entity.InventoryEntity;
import foundation.softwaredesign.orbi.persistence.repo.game.character.InventoryRepository;
import foundation.softwaredesign.orbi.persistence.repo.game.gameobject.GameObjectTypeMappper;
import foundation.softwaredesign.orbi.service.auth.UserService;
import foundation.softwaredesign.orbi.service.game.gameobject.GameObjectTypeCategoryService;
import foundation.softwaredesign.orbi.service.game.gameobject.GameObjectTypeService;

import javax.enterprise.context.RequestScoped;
import javax.inject.Inject;
import javax.ws.rs.NotFoundException;
import java.util.ArrayList;
import java.util.Calendar;
import java.util.List;
import java.util.Objects;
import java.util.concurrent.ThreadLocalRandom;

/**
 * @author Lucas Reeh <lr86gm@gmail.com>
 */
@RequestScoped
public class InventoryService {

    private static final String ALWAYS_RESTOCK_OBJECT_TYPE_PREFAB = "Cubes/Bricks";
    private static final Integer MAX_RARITY = 3;

    @Inject
    InventoryRepository repository;
    @Inject
    UserService userService;
    @Inject
    GameObjectTypeService gameObjectType;
    @Inject
    GameObjectTypeCategoryService gameObjectTypeCategory;
    @Inject
    CharacterService characterService;


    public void checkForGiftChest(GameObject object) {
        if (gameObjectType.isGiftObject(object.getType())) {
            addRandomGifts();
        }
    }

    private void addRandomGifts() {
        // TODO find better algo for rarity -> probability
        List<GameObjectType> types = gameObjectType.loadAllCraftable();
        List<GameObjectType> allIds = new ArrayList<>();
        for (GameObjectType type : types) {
            for (Integer i = 1; i < (((MAX_RARITY + 1) - type.getRarity()) * 10); i++) {
                allIds.add(type);
            }
        }
        Integer randomIndex = ThreadLocalRandom.current().nextInt(0, types.size());
        GameObjectType randomType = types.get(randomIndex);
        addItem(randomType.getPrefab(), new Long(ThreadLocalRandom.current().nextInt(1, randomType.getSpawnAmount())));
    }


    public void checkBasicInventoryAndRestock() {
        InventoryEntity inventoryEntity =
                repository.findByIdentAndType(userService.getIdentity().getId(),
                        gameObjectType.loadByPrefab(ALWAYS_RESTOCK_OBJECT_TYPE_PREFAB).getId());

        if (Objects.isNull(inventoryEntity) || (Objects.nonNull(inventoryEntity) && inventoryEntity.getAmount().longValue() <= 0)) {
            addItem(ALWAYS_RESTOCK_OBJECT_TYPE_PREFAB, new Long(25));
        }
    }

    public Inventory getInventory() {
        Inventory inventory = new Inventory();
        Calendar cal = Calendar.getInstance();
        cal.add(Calendar.HOUR, -3);
        if (userService.getIdentity().getLastInit().before(cal.getTime())) {
            characterService.incrementXp(CharacterDevelopment.XP_LOGIN);
            addRandomGifts();
        }

        for (InventoryEntity inventoryEntity : repository.findByIdentityId(userService.getIdentity().getId())) {
            InventoryItem newItem = new InventoryItem();
            newItem.setAmount(inventoryEntity.getAmount());
            newItem.setType(new GameObjectTypeMappper().toDto(inventoryEntity.getGameObjectType()));
            newItem.setCategoryId(inventoryEntity.getGameObjectType().getGameObjectTypeCategoryEntity().getId());
            newItem.setSupportsUserText(inventoryEntity.getGameObjectType().getSupportsUserText());
            inventory.getItems().add(newItem);
        }
        inventory.setCategories(gameObjectTypeCategory.loadAll());
        return inventory;
    }

    public void use(GameObject gameObject) {
        InventoryEntity toUse = getInventoryItemsByPrefab(gameObject.getType().getPrefab());
        if (Objects.isNull(toUse) || (Objects.nonNull(toUse) && (toUse.getAmount().longValue() <= 0))) {
            throw new NotFoundException("Item not in inventory");
        }
        toUse.setAmount(toUse.getAmount() - 1);
        repository.save(toUse);
    }

    public void addItem(String prefab, Long amount) {
        // get object type by prefab
        GameObjectType objectType = gameObjectType.loadByPrefab(prefab);
        InventoryEntity inventory = repository.findByIdentAndType(userService.getIdentity().getId(), objectType.getId());
        if (Objects.isNull(inventory)) {
            inventory = new InventoryEntity();
            inventory.setGameObjectType(new GameObjectTypeMappper().toEntity(null,objectType));
            inventory.setIdentity(userService.getIdentity());
            inventory.setAmount(new Long(0));
        }
        inventory.setAmount(inventory.getAmount() + amount);
        repository.saveAndFlush(inventory);
    }

    private InventoryEntity getInventoryItemsByPrefab(String prefab) {
        GameObjectType objectType = gameObjectType.loadByPrefab(prefab);
        InventoryEntity inventoryItems =
                repository.findByIdentAndType(userService.getIdentity().getId(), objectType.getId());
        return inventoryItems;
    }
}
