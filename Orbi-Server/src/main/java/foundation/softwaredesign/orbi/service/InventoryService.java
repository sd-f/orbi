package foundation.softwaredesign.orbi.service;

import foundation.softwaredesign.orbi.model.CharacterDevelopment;
import foundation.softwaredesign.orbi.model.GameObject;
import foundation.softwaredesign.orbi.model.Inventory;
import foundation.softwaredesign.orbi.model.InventoryItem;
import foundation.softwaredesign.orbi.persistence.entity.GameObjectTypeEntity;
import foundation.softwaredesign.orbi.persistence.entity.InventoryEntity;
import foundation.softwaredesign.orbi.persistence.repo.InventoryRepository;

import javax.enterprise.context.RequestScoped;
import javax.inject.Inject;
import javax.ws.rs.NotFoundException;
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
    private static final String GIFT_CHEST_OBJECT_TYPE_PREFAB_PREFIX = "ScifiCrate/ScifiCrate_";
    private static final String GIFT_CHEST_OBJECT_TYPE_PREFAB = "ScifiCrate/ScifiCrate_1";
    @Inject
    InventoryRepository repository;
    @Inject
    UserService userService;
    @Inject
    GameObjectTypeService gameObjectType;
    @Inject
    CharacterService characterService;

    public void checkForGiftChest(GameObject object) {
        if (object.getPrefab().startsWith(GIFT_CHEST_OBJECT_TYPE_PREFAB_PREFIX)) {
            List<GameObjectTypeEntity> types = gameObjectType.loadAll();
            for (int i = 0; i < 3; i++) {
                Integer randomIndex = ThreadLocalRandom.current().nextInt(0,types.size());
                addItem(types.get(randomIndex).getPrefab(),new Long(ThreadLocalRandom.current().nextInt(1,10)));
            }
        }
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
            addItem(GIFT_CHEST_OBJECT_TYPE_PREFAB,new Long(1));
        }

        for (InventoryEntity inventoryEntity: repository.findByIdentityId(userService.getIdentity().getId())) {
            inventory.getItems().add(new InventoryItem(inventoryEntity.getGameObjectType().getPrefab(), inventoryEntity.getAmount(), inventoryEntity.getGameObjectType().getSupportsUserText()));
        }
        inventory.setNumberOfObjectTypes(gameObjectType.getNumberOfObjectTypes());
        return inventory;
    }

    public void use(GameObject gameObject) {
        InventoryEntity toUse = getInventoryItemsByPrefab(gameObject.getPrefab());
        if (Objects.isNull(toUse) || (Objects.nonNull(toUse) && (toUse.getAmount().longValue() <= 0))) {
            throw new NotFoundException("Item not in inventory");
        }
        toUse.setAmount(toUse.getAmount() - 1);
        repository.save(toUse);
    }

    public void addItem(String prefab, Long amount) {
        // get object type by prefab
        GameObjectTypeEntity objectTypeEntity = gameObjectType.loadByPrefab(prefab);
        InventoryEntity inventory = repository.findByIdentAndType(userService.getIdentity().getId(),objectTypeEntity.getId());
        if (Objects.isNull(inventory)) {
            inventory = new InventoryEntity();
            inventory.setGameObjectType(objectTypeEntity);
            inventory.setIdentity(userService.getIdentity());
            inventory.setAmount(new Long(0));
        }
        inventory.setAmount(inventory.getAmount() + amount);
        repository.saveAndFlush(inventory);
    }

    private InventoryEntity getInventoryItemsByPrefab(String prefab) {
        GameObjectTypeEntity objectType = gameObjectType.loadByPrefab(prefab);
        InventoryEntity inventoryItems =
                repository.findByIdentAndType(userService.getIdentity().getId(), objectType.getId());
        return inventoryItems;
    }
}
