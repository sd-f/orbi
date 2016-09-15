package foundation.softwaredesign.orbi.service;

import foundation.softwaredesign.orbi.model.*;
import foundation.softwaredesign.orbi.persistence.entity.InventoryEntity;
import foundation.softwaredesign.orbi.persistence.entity.InventoryEntityId;
import foundation.softwaredesign.orbi.persistence.repo.GameObjectRepository;
import foundation.softwaredesign.orbi.persistence.repo.GameObjectTypeRepository;
import foundation.softwaredesign.orbi.persistence.repo.InventoryRepository;

import javax.enterprise.context.RequestScoped;
import javax.inject.Inject;
import java.util.Date;
import java.util.Objects;

/**
 * @author Lucas Reeh <lr86gm@gmail.com>
 */
@RequestScoped
public class InventoryService {

    private static final Long ALWAYS_RESTOCK_OBJECT_TYPE_ID = new Long(1);

    @Inject
    UserService userService;
    @Inject
    InventoryRepository inventoryRepository;
    @Inject
    GameObjectTypeRepository gameObjectTypeRepository;

    public void checkRestock() {
        InventoryEntity inventoryEntity =
                inventoryRepository.findByIdentityIdAndGameObjectTypeId(userService.getIdentity().getId(),
                        ALWAYS_RESTOCK_OBJECT_TYPE_ID);

        if (Objects.isNull(inventoryEntity) || (Objects.nonNull(inventoryEntity) && inventoryEntity.getAmount().longValue() <= 0)) {
            restock(inventoryEntity);
        }
    }

    private void restock(InventoryEntity inventoryEntity) {
        InventoryEntity inventoryEntityRestocked = inventoryEntity;
        if (Objects.isNull(inventoryEntityRestocked)) {
            inventoryEntityRestocked = new InventoryEntity();
            inventoryEntityRestocked.setGameObjectType(gameObjectTypeRepository.findBy(ALWAYS_RESTOCK_OBJECT_TYPE_ID));
            inventoryEntityRestocked.setIdentity(userService.getIdentity());
            inventoryEntityRestocked.setAmount(new Long(0));
        }
        inventoryEntityRestocked.setAmount(inventoryEntityRestocked.getAmount() + 100);
        inventoryRepository.saveAndFlush(inventoryEntityRestocked);
    }

    public Inventory getInventory() {
        Inventory inventory = new Inventory();
        for (InventoryEntity inventoryEntity: inventoryRepository.findByIdentityId(userService.getIdentity().getId())) {
            inventory.getItems().add(new InventoryItem(inventoryEntity.getGameObjectType().getPrefab(), inventoryEntity.getAmount()));
        }
        return inventory;
    }
}
