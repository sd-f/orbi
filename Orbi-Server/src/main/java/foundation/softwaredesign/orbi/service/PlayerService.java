package foundation.softwaredesign.orbi.service;

import foundation.softwaredesign.orbi.model.*;

import javax.enterprise.context.RequestScoped;
import javax.inject.Inject;

/**
 * @author Lucas Reeh <lr86gm@gmail.com>
 */
@RequestScoped
public class PlayerService {

    @Inject
    WorldService world;
    @Inject
    UserService user;
    @Inject
    InventoryService inventory;
    @Inject
    GameObjectService gameObjectService;

    public World craft(Player player) {
        user.updatePosition(player.getGeoPosition());
        inventory.checkBasicInventoryAndRestock();
        inventory.use(player.getGameObjectToCraft());
        player.getGameObjectToCraft().setIdentityId(user.getIdentity().getId());
        world.create(player.getGameObjectToCraft());
        return world.getWorld(player.getGeoPosition());
    }

    public World destroy(Player player) {
        user.updatePosition(player.getGeoPosition());
        Long id = player.getSelectedObjectId();
        GameObject object = gameObjectService.findById(id);
        inventory.addItem(object.getPrefab(), new Long(1));
        inventory.checkForGiftChest(object);
        world.delete(object.getId());
        return world.getWorld(player.getGeoPosition());
    }

    public Inventory getInventory() {
        inventory.checkBasicInventoryAndRestock();
        return inventory.getInventory();
    }

}
