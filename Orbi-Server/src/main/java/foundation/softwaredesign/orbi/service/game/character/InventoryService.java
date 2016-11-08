package foundation.softwaredesign.orbi.service.game.character;

import foundation.softwaredesign.orbi.model.game.character.CharacterDevelopment;
import foundation.softwaredesign.orbi.model.game.character.Inventory;
import foundation.softwaredesign.orbi.model.game.character.InventoryItem;
import foundation.softwaredesign.orbi.model.game.gameobject.GameObject;
import foundation.softwaredesign.orbi.model.game.gameobject.GameObjectType;
import foundation.softwaredesign.orbi.persistence.entity.InventoryEntity;
import foundation.softwaredesign.orbi.persistence.repo.game.character.InventoryRepository;
import foundation.softwaredesign.orbi.persistence.repo.game.gameobject.GameObjectTypeMappper;
import foundation.softwaredesign.orbi.service.auth.UserService;
import foundation.softwaredesign.orbi.service.game.gameobject.GameObjectTypeCategoryService;
import foundation.softwaredesign.orbi.service.game.gameobject.GameObjectTypeService;
import foundation.softwaredesign.orbi.service.game.server.DateConverter;

import javax.enterprise.context.RequestScoped;
import javax.inject.Inject;
import javax.ws.rs.NotFoundException;
import java.util.*;
import java.util.concurrent.ThreadLocalRandom;
import java.util.stream.Collectors;

/**
 * @author Lucas Reeh <lr86gm@gmail.com>
 */
@RequestScoped
public class InventoryService {

    private static final String ALWAYS_RESTOCK_OBJECT_TYPE_PREFAB = "Cubes/Bricks";
    public static final Integer MAX_RARITY = 4;

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
    @Inject
    DateConverter date;


    public void checkForGiftChest(GameObject object) {
        if (gameObjectType.isGiftObject(object.getType())) {
            addRandomGifts();
        }
    }

    private void addRandomGifts() {
        List<GameObjectType> types = gameObjectType.loadAllCraftable();
        GameObjectType randomType = getRandomTypeByRarity(types);
        Long randomAmount = new Long(1);
        if (randomType.getSpawnAmount() > 1)
            randomAmount = new Long(ThreadLocalRandom.current().nextInt(1, randomType.getSpawnAmount()));
        addItem(randomType.getPrefab(), randomAmount);
    }

    public GameObjectType getRandomTypeByRarity(List<GameObjectType> types) {
        // TODO find better algo for rarity -> probability
        List<GameObjectType> allIds = new ArrayList<>();
        for (GameObjectType type : types) {
            for (Integer i = 1; i < (((InventoryService.MAX_RARITY + 1) - type.getRarity()) * 10); i++) {
                allIds.add(type);
            }
        }
        // TODO will fail if types param is empty
        Integer randomIndex = ThreadLocalRandom.current().nextInt(0, types.size());
        GameObjectType randomType = types.get(randomIndex);
        return randomType;
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
        List<InventoryEntity> items = loadCraftable();
        for (InventoryEntity inventoryEntity : items) {
            InventoryItem newItem = new InventoryItem();
            newItem.setAmount(inventoryEntity.getAmount());
            newItem.setType(new GameObjectTypeMappper().toDto(inventoryEntity.getGameObjectType()));
            newItem.setCategoryId(inventoryEntity.getGameObjectType().getGameObjectTypeCategoryEntity().getId());
            newItem.setSupportsUserText(inventoryEntity.getGameObjectType().getSupportsUserText());
            newItem.setDiscoveredOn(date.toString(inventoryEntity.getDiscoveredOn()));
            inventory.getItems().add(newItem);
        }
        inventory.setCategories(gameObjectTypeCategory.loadAll());
        return inventory;
    }

    private List<InventoryEntity> loadCraftable() {
        return repository.findByIdentityId(userService.getIdentity().getId())
                .stream()
                .filter(inventoryEntity -> inventoryEntity.getGameObjectType().getGameObjectTypeCategoryEntity().getCraftable())
                .collect(Collectors.toList());
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
            inventory.setDiscoveredOn(new Date());
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
