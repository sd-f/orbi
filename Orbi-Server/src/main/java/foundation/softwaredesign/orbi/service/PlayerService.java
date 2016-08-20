package foundation.softwaredesign.orbi.service;

import foundation.softwaredesign.orbi.model.CraftingGameObject;
import foundation.softwaredesign.orbi.model.GameObject;
import foundation.softwaredesign.orbi.model.World;
import foundation.softwaredesign.orbi.persistence.repo.GameObjectRepository;

import javax.enterprise.context.RequestScoped;
import javax.inject.Inject;

/**
 * @author Lucas Reeh <lr86gm@gmail.com>
 */
@RequestScoped
public class PlayerService {

    @Inject
    GameObjectRepository gameObjectRepository;

    @Inject
    WorldService worldService;

    private void saveGameObject(GameObject gameObject) {
        gameObjectRepository.saveAndFlush(gameObject);
    }

    public World craftGameObject(CraftingGameObject craftingGameObject) {
        saveGameObject(craftingGameObject.getGameObject());
        return worldService.getWorld(craftingGameObject.getPlayerGeoPosition());
    }
}
