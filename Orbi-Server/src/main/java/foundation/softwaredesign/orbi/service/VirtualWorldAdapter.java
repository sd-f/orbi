package foundation.softwaredesign.orbi.service;

import foundation.softwaredesign.orbi.model.virtual.GameObject;
import foundation.softwaredesign.orbi.model.virtual.Position;
import foundation.softwaredesign.orbi.model.virtual.World;

import javax.enterprise.context.RequestScoped;

/**
 * @author Lucas Reeh <lr86gm@gmail.com>
 */
@RequestScoped
public class VirtualWorldAdapter extends WorldAdapter {

    /**
     * @param world  with virtual positions
     * @param center virtual position
     */
    public void translate(World world, Position center) {
        if (isWorldOk(world)) {
            world.getGameObjects()
                    .stream()
                    .filter(this::isCubeOk)
                    .forEach(gameObject -> translatePosition(gameObject, center));
        }
    }

    /**
     * @param world with real positions
     */
    public void scale(World world) {
        if (isWorldOk(world)) {
            world.getGameObjects()
                    .stream()
                    .filter(this::isCubeOk)
                    .forEach(gameObject -> scalePosition(gameObject.getPosition()));
        }
    }

    public void translatePosition(GameObject gameObject, Position center) {
        if (isPositionOk(gameObject.getPosition())) {
            Double diffLatitude = (gameObject.getPosition().getZ() - center.getZ());
            Double diffLongitude = (gameObject.getPosition().getX() - center.getX());
            gameObject.getPosition().setZ(diffLatitude);
            gameObject.getPosition().setX(diffLongitude);
        }
    }

    public void scalePosition(Position position) {

        if (isPositionOk(position)) {
            Double mPerPixel = 156543.03392 * Math.cos(position.getZ() * Math.PI / 180.0) / Math.pow(2.0, 18);
            position.setZ((position.getZ()) * mPerPixel * 64000.0);
            position.setX((position.getX()) * mPerPixel * 64000.0);
            position.setY(position.getY());
        }

    }

}
