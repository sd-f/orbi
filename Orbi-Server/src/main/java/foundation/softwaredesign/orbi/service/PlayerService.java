package foundation.softwaredesign.orbi.service;

import foundation.softwaredesign.orbi.model.*;
import foundation.softwaredesign.orbi.persistence.entity.InventoryEntity;
import foundation.softwaredesign.orbi.persistence.repo.GameObjectRepository;
import foundation.softwaredesign.orbi.persistence.repo.InventoryRepository;

import javax.enterprise.context.RequestScoped;
import javax.inject.Inject;
import java.util.Date;
import java.util.List;

/**
 * @author Lucas Reeh <lr86gm@gmail.com>
 */
@RequestScoped
public class PlayerService {

    @Inject
    GameObjectRepository gameObjectRepository;
    @Inject
    WorldService worldService;
    @Inject
    UserService userService;
    @Inject
    InventoryService inventoryService;

    private void saveGameObject(GameObject gameObject) {
        gameObject.setCreateDate(new Date());
        gameObjectRepository.saveAndFlush(gameObject);
    }

    public World craftGameObject(Player player) {
        userService.updatePosition(player.getGeoPosition());
        saveGameObject(player.getGameObjectToCraft());
        return worldService.getWorld(player.getGeoPosition());
    }

    public Inventory getInventory() {
        inventoryService.checkRestock();
        return inventoryService.getInventory();
    }
}
