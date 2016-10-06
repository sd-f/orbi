package foundation.softwaredesign.orbi.service;

import foundation.softwaredesign.orbi.model.*;
import foundation.softwaredesign.orbi.model.Character;

import javax.enterprise.context.RequestScoped;
import javax.inject.Inject;
import javax.ws.rs.BadRequestException;

import static java.util.Objects.isNull;

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
    @Inject
    CharacterService characterService;

    public World craft(Player player) {
        characterService.updateTransform(player.getCharacter().getTransform());
        inventory.checkBasicInventoryAndRestock();
        inventory.use(player.getGameObjectToCraft());
        player.getGameObjectToCraft().setIdentityId(user.getIdentity().getId());
        world.create(player.getGameObjectToCraft());
        return world.getWorld(player.getCharacter().getTransform().getGeoPosition());
    }

    public World destroy(Player player) {
        characterService.updateTransform(player.getCharacter().getTransform());
        Long id = player.getSelectedObjectId();
        GameObject object = gameObjectService.findById(id);
        inventory.addItem(object.getPrefab(), new Long(1));
        inventory.checkForGiftChest(object);
        world.delete(object.getId());
        return world.getWorld(player.getCharacter().getTransform().getGeoPosition());
    }

    public Inventory getInventory() {
        inventory.checkBasicInventoryAndRestock();
        return inventory.getInventory();
    }

    /**
     *
     * @param newTransform initial or update position and rotation
     * @return
     */
    public Player init(Transform newTransform) {
        Character currentCharacter = characterService.updateTransform(newTransform);
        Player player = new Player();
        player.setCharacter(currentCharacter);
        return player;
    }
}
