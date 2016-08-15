package foundation.softwaredesign.orbi.service;

import foundation.softwaredesign.orbi.model.real.Position;
import foundation.softwaredesign.orbi.model.virtual.GameObject;
import foundation.softwaredesign.orbi.model.virtual.World;

import javax.enterprise.context.ApplicationScoped;
import javax.enterprise.context.RequestScoped;

import java.math.BigDecimal;

import static java.util.Objects.nonNull;

/**
 * @author Lucas Reeh <lr86gm@gmail.com>
 */
@RequestScoped
public class WorldAdapter {

    private Boolean isWorldOk(World world) {
        if (nonNull(world) && nonNull(world.getGameObjects()) && !world.getGameObjects().isEmpty()) {
            return true;
        }
        return false;
    }

    private Boolean isPositionOk(Position position) {
        if (nonNull(position)) {
            if (nonNull(position.getElevation())
                    && nonNull(position.getLatitude())
                && nonNull(position.getLongitute())) {
                return true;
            }
        }
        return false;
    }

    private Boolean isCubeOk(GameObject gameObject) {
        if (nonNull(gameObject) && nonNull(gameObject.getPosition())) {
            if (nonNull(gameObject.getPosition().getX())
                    && nonNull(gameObject.getPosition().getY())
                    && nonNull(gameObject.getPosition().getZ())) {
                return true;
            }
        }
        return false;
    }

    private BigDecimal scaleToVirtual(BigDecimal real, BigDecimal position, Integer scale) {
        BigDecimal newCoordinate = real.subtract(position);
        return newCoordinate.multiply(new BigDecimal(scale));
    }

    private BigDecimal scaleToReal(BigDecimal virtual, BigDecimal position, Integer scale) {
        BigDecimal newCoordinate = virtual.divide(new BigDecimal(scale));
        return newCoordinate.add(position);
    }

    public void convertToVirtual(World world, Position position) {
        if (isWorldOk(world) && isPositionOk(position)) {
            world.getGameObjects()
                    .stream()
                    .filter(this::isCubeOk)
                    .forEach(cube -> {
                        cube.getPosition().setZ(
                                scaleToVirtual(cube.getPosition().getZ(), position.getLongitute(), 50000).multiply(BigDecimal.valueOf(-1f))
                        );
                        cube.getPosition().setY(
                                scaleToVirtual(cube.getPosition().getY(), position.getElevation(), 1)
                        );
                        cube.getPosition().setX(
                                scaleToVirtual(cube.getPosition().getX(), position.getLatitude(), 50000)
                        );
            });
        }
    }

    public void convertToReal(World world, Position position) {
        if (isWorldOk(world) && isPositionOk(position)) {
            world.getGameObjects()
                    .stream()
                    .filter(this::isCubeOk)
                    .forEach(cube -> {
                        cube.getPosition().setZ(
                                scaleToReal(cube.getPosition().getZ(), position.getLongitute(), 50000)
                        );
                        cube.getPosition().setY(
                                scaleToReal(cube.getPosition().getY(), position.getElevation(), 1)
                        );
                        cube.getPosition().setX(
                                scaleToReal(cube.getPosition().getX(), position.getLatitude(), 50000)
                        );
                    });
        }
    }
}
